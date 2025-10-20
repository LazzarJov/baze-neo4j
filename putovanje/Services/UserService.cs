using Neo4jClient;
using Neo4jClient.Cypher;
using putovanjeApp1.Dtos;
using putovanjeApp1.Models;
using System.Xml.Serialization;
using BCrypt.Net;

namespace putovanjeApp1.Services
{
    public class UserService
    {
        private readonly IGraphClient _client;

        public UserService(IGraphClient client)
        {
            _client = client;
        }



    public async Task<User> RegisterAsync(User user)
        {
            user.Guid = Guid.NewGuid();

            var provided = user.PasswordHash ?? string.Empty;
            string passwordHash;

            if (string.IsNullOrWhiteSpace(provided))
            {
                throw new ArgumentException("Password must be provided in user.PasswordHash (plain) for registration.");
            }

         
            if (provided.StartsWith("$2a$") || provided.StartsWith("$2b$") || provided.StartsWith("$2y$"))
            {
                passwordHash = provided;
            }
            else
            {
                // heširaj plain lozinku
                passwordHash = BCrypt.Net.BCrypt.HashPassword(provided);
            }

            // 3) Upiši u Neo4j - koristimo konzistentna imena property-ja (npr. mala slova)
            //    (Neo4j property imena su case-sensitive, pa koristi jedno pravilo u čitavom projektu)
            await _client.Cypher
                .Create("(u:User $user)")
                .WithParam("user", new
                {
                    guid = user.Guid.ToString(),   // čuvamo GUID kao string u bazi
                    ime = user.Ime,
                    email = user.Email,
                    passwordHash = passwordHash,
                    interesovanja = user.Interesovanja ?? new List<string>()
                })
                .ExecuteWithoutResultsAsync();

            // 4) Sakrij password pre vraćanja objekta nazad
            user.PasswordHash = null;

            return user;
        }


    // Login - vraća Guid korisnika ako postoji
    public async Task<Guid?> LoginAsync(string email, string password)
        {
            var result = await _client.Cypher
                .Match("(u:User)")
                .Where((User u) => u.Email == email && u.PasswordHash == password)
                .Return(u => u.As<User>().Guid)
                .ResultsAsync;

            return result.FirstOrDefault();
        }

        public async Task<User?> GetByGuidAsync(Guid guid)
        {
            var results = await _client.Cypher
                .Match("(u:User)")
                .Where("u.Guid = $guid")
                .WithParam("guid", guid.ToString())
                .Return(u => u.As<User>())
                .ResultsAsync;

            return results.FirstOrDefault();
        }

        public async Task<bool> UpdateAsync(Guid guid, UserDto updateDto)
        {
            // Ne mapiramo passwordHash ovde; za promenu lozinke imaj poseban endpoint
            var props = new
            {
                Ime = updateDto.ime,
                Email = updateDto.email,
                Interesovanja = updateDto.interesovanja
            };

                 await _client.Cypher
                .Match("(u:User)")
                .Where("u.Guid = $guid")
                .WithParam("guid", guid.ToString())
                .Set("u.Ime = $Ime, u.Email = $Email, u.Interesovanja = $Interesovanja")
                .WithParams(props)
                .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            await _client.Cypher
                .Match("(u:User)")
                .Where("u.Guid = $guid")
                .WithParam("guid", guid.ToString())
                .DetachDelete("u")
                .ExecuteWithoutResultsAsync();

            return true;
        }




        // preporuka destinacija po interesovanjima korisnika
        public async Task<List<Destinacija>> GetRecommendedDestinations(Guid userId)
        {
            var destinations = await _client.Cypher
                .Match("(u:User)-[:VOLI]->(a:Aktivnost)<-[:NUDI]-(d:Destinacija)")
                .Where((User u) => u.Guid == userId)
                .Return(d => d.As<Destinacija>())
                .ResultsAsync;

            return destinations.ToList();
        }


        // preporuka atrakcija ili aktivnosti korisniku
        public async Task<List<Atrakcija>> GetRecommendedActivities(Guid userGuid)
        {
            var activities = await _client.Cypher
                // 1️⃣ Korisnik -> BIO_NA -> Putovanje -> OBUHVATA -> Destinacija -> IMA_ATRAKCIJU -> Atrakcija
                .Match("(u:User)-[:BIO_NA]->(p:Putovanje)-[:OBUHVATA]->(d:Destinacija)-[:IMA_ATRAKCIJU]->(at:Atrakcija)")
                .Where((User u) => u.Guid == userGuid)
                .With("u, at, d")

                // 2️⃣ Drugi korisnici koji su bili na istim destinacijama
                .Match("(other:User)-[:BIO_NA]->(:Putovanje)-[:OBUHVATA]->(d)-[:IMA_ATRAKCIJU]->(at)")
                .Where("other.guid <> $userGuid")
                .WithParam("userGuid", userGuid)

                // 3️⃣ Vraćamo jedinstvene atrakcije
                .ReturnDistinct(at => at.As<Atrakcija>())
                .ResultsAsync;

            return activities.ToList();
        }


        public async Task<List<Destinacija>> GetRecommendedDestinationsAsync(Guid userGuid, int limit = 10)
        {
            // 1️⃣ Pronađi slične korisnike po zajedničkim aktivnostima i putovanjima
            var similarUsers = await _client.Cypher
                .Match("(u:User {guid: $userGuid})-[:VOLI|BIO_NA]->(x)<-[:VOLI|BIO_NA]-(other:User)")
                .WithParam("userGuid", userGuid)
                .ReturnDistinct(other => other.As<User>().Guid)
                .ResultsAsync;

            var similarUserGuids = similarUsers.ToList();

            if (!similarUserGuids.Any())
                return new List<Destinacija>(); // nema sličnih korisnika

            // 2️⃣ Pronađi destinacije koje slični korisnici posećuju, a korisnik nije
            var recommended = await _client.Cypher
                .Match("(other:User)-[:BIO_NA]->(:Putovanje)-[:OBUHVATA]->(d:Destinacija)")
                .Where("other.guid IN $similarUserGuids")
                .AndWhere("(NOT (:User {guid: $userGuid})-[:BIO_NA]->(:Putovanje)-[:OBUHVATA]->(d))")
                .WithParam("similarUserGuids", similarUserGuids)
                .WithParam("userGuid", userGuid)
                .Return((d, other) => new
                {
                    destinacija = d.As<Destinacija>(),
                    popularity = "count(other)" // ručno zadat COUNT
                })
                .OrderByDescending("count(other)")
                .Limit(limit)
                .ResultsAsync;

            // 3️⃣ Vraćamo samo listu destinacija
            return recommended.Select(r => r.destinacija).ToList();
        }









    }
}
