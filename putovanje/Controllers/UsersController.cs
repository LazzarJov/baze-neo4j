using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Neo4jClient.Cypher;
using putovanjeApp1.Dtos;
using putovanjeApp1.Models;
using putovanjeApp1.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace putovanjeApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IGraphClient _client;

        private readonly UserService _userService;

        public UserController(IGraphClient client, UserService userService)
        {
            _client = client;
            _userService = userService;
        }

        private Guid? GetCurrentUserGuid()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return null;
            if (Guid.TryParse(claim.Value, out var g)) return g;
            return null;
        }

        [HttpGet("{guid:guid}")]
        [Authorize]
        public async Task<IActionResult> GetPrivateInfo(Guid guid)
        {
            var current = GetCurrentUserGuid();
            if (current == null) return Unauthorized();

            // dozvoljeno ako je vlasnik ili ima ulogu Admin
            if (current != guid && !User.IsInRole("Admin"))
                return Forbid();

            var user = await _userService.GetByGuidAsync(guid);
            if (user == null) return NotFound();

            // Sakrij passwordHash pre slanja
            user.passwordHash = null;
            return Ok(user);
        }

        [HttpPut("{guid}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid guid, [FromBody] UserDto dto)
        {
            var current = GetCurrentUserGuid();
            if (current == null) return Unauthorized();
            if (current != guid && !User.IsInRole("Admin"))
                return Forbid();

            // Po potrebi validiraj dto (email format itd.)
            var ok = await _userService.UpdateAsync(guid, dto);
            if (!ok) return BadRequest();

            return NoContent();
        }

        // DELETE: samo vlasnik ili admin
        [HttpDelete("{guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var current = GetCurrentUserGuid();
            if (current == null) return Unauthorized();
            if (current != guid && !User.IsInRole("Admin"))
                return Forbid();

            await _userService.DeleteAsync(guid);
            return NoContent();
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _client.Cypher
                .Match("(u:User)")
                .Return(u => u.As<User>())
                .ResultsAsync;

            return Ok(users.ToList());
        }

        [HttpGet("public/{guid:guid}")]
        public async Task<IActionResult> GetById(Guid guid)
        {
            var users = await _client.Cypher
                .Match("(u:User)")
                .Where((User u) => u.guid == guid)
                .Return(u => u.As<User>())
                .ResultsAsync;

            var result = users.SingleOrDefault();
            return result != null ? Ok(result) : NotFound($"User sa ID {guid} nije pronađen.");
        }


        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User noviUser)
        {
            await _client.Cypher
                .Create("(u:User $noviUser)")
                .WithParam("noviUser", noviUser)
                .ExecuteWithoutResultsAsync();

            return Ok("User uspešno dodat.");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User izmenjeniUser)
        {
            await _client.Cypher
                .Match("(u:User)")
                .Where((User u) => u.guid == id)
                .Set("u = $izmenjeniUser")
                .WithParam("izmenjeniUser", izmenjeniUser)
                .ExecuteWithoutResultsAsync();

            return Ok("User uspešno izmenjen.");
        }

        
    



        //// GET: api/user/{id}/recommendations/destinations
        //[HttpGet("{id}/recommendations/destinations")]
        //public async Task<IActionResult> GetRecommendedDestinations(Guid id)
        //{
        //    var preporuke = await _client.Cypher
        //        .Match("(u:User)-[:LIKES]->(a:Aktivnost)<-[:NUDI]-(at:Atrakcija)<-[:SADRZI]-(d:Destinacija)")
        //        .Where((User u) => u.guid == id)
        //        .Return(d => d.As<Destinacija>())
        //        .ResultsAsync;

        //    var result = preporuke.Distinct().ToList();
        //    return result.Any() ? Ok(result) : NotFound($"Nema preporučenih destinacija za korisnika sa ID {id}.");
        //}

        //// GET: api/user/{id}/recommendations/activities
        //[HttpGet("{id}/recommendations/activities")]
        //public async Task<IActionResult> GetRecommendedActivities(Guid id)
        //{
        //    var preporuke = await _client.Cypher
        //        .Match("(u:User)-[:LIKES]->(a:Aktivnost)<-[:NUDI]-(at:Atrakcija)<-[:SADRZI]-(d:Destinacija)")
        //        .Where((User u) => u.guid == id)
        //        .OptionalMatch("(u)-[:BEO_NA]->(p:Putovanje)-[:SADRZI]->(d2:Destinacija)-[:NUDI]->(at2:Atrakcija)")
        //        .With("collect(at2) as posetio, at, d")
        //        .Where("NOT at IN posetio")
        //        .Return(at => at.As<Aktivnost>())
        //        .ResultsAsync;

        //    var result = preporuke.Distinct().ToList();
        //    return result.Any() ? Ok(result) : NotFound($"Nema preporučenih aktivnosti za korisnika sa ID {id}.");
        //}

        //[HttpGet("{id}/similar")]
        //public async Task<IActionResult> GetSimilarUsers(Guid id)
        //{
        //    var similarUsers = await _client.Cypher
        //        .Match("(u:User {Id: $userId})-[:LIKES|PUTOVAO_NA]->(x)")
        //        .Match("(other:User)-[:LIKES|PUTOVAO_NA]->(x)")
        //        .Where("other.Id <> $userId")
        //        .WithParam("userId", id)
        //        .Return((other, x) => new {
        //            User = other.As<User>(),
        //            CommonCount = Return.As<int>("COUNT(x)")
        //        })
        //        .OrderByDescending("CommonCount")
        //        .Limit(10)
        //        .ResultsAsync;

        //    return Ok(similarUsers);
        //}



        //[HttpGet("{id}/recommendations/destinations2")]
        //public async Task<IActionResult> GetRecommendedDestinations2(Guid id)
        //{
        //    var preporuke = await _userService.GetRecommendedDestinationsAsync(id);
        //    return Ok(preporuke);
        //}
        [HttpGet("{id}/recommendations/destinations")]
        public async Task<IActionResult> GetRecommendedDestinations(Guid id)
        {
            var preporuke = await _userService.GetRecommendedDestinations(id);
            return preporuke.Any()
                ? Ok(preporuke)
                : NotFound($"Nema preporučenih destinacija za korisnika sa ID {id}.");
        }

        [HttpGet("{id}/recommendations/activities")]
        public async Task<IActionResult> GetRecommendedActivities(Guid id)
        {
            var preporuke = await _userService.GetRecommendedActivities(id);
            return preporuke.Any()
                ? Ok(preporuke)
                : NotFound($"Nema preporučenih aktivnosti za korisnika sa ID {id}.");
        }

        [HttpGet("{id}/recommendations/destinations2")]
        public async Task<IActionResult> GetRecommendedDestinations2(Guid id)
        {
            var preporuke = await _userService.GetRecommendedDestinationsAsync(id);
            return preporuke.Any()
                ? Ok(preporuke)
                : NotFound($"Nema preporuka za korisnika sa ID {id}.");
        }




    }
}
