//using Neo4j.Driver;
//using putovanjeApp1.Models;


//namespace putovanjeApp1.Services;

//public class Neo4jService
//{
//    private readonly IDriver _driver;

//    public Neo4jService(IDriver driver)
//    {
//        _driver = driver;
//    }

//    private IAsyncSession GetSession() => _driver.AsyncSession();

//    // ================= USERS =================
//    public async Task<List<User>> GetUsersAsync()
//    {
//        var users = new List<User>();
//        var session = GetSession();

//        try
//        {
//            var cursor = await session.RunAsync("MATCH (u:User) RETURN u");
//            await cursor.ForEachAsync(record =>
//            {
//                var node = record["u"].As<INode>();
//                users.Add(new User
//                {
//                    Id = node.Properties.ContainsKey("id") ? node.Properties["id"].As<string>() : "",
//                    Ime = node.Properties.ContainsKey("ime") ? node.Properties["ime"].As<string>() : "",
//                    Email = node.Properties.ContainsKey("email") ? node.Properties["email"].As<string>() : ""
//                });
//            });
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }

//        return users;
//    }

//    public async Task CreateUserAsync(User user)
//    {
//        var session = GetSession();
//        try
//        {
//            await session.RunAsync(
//                "CREATE (u:User {id:$id, ime:$ime, email:$email})",
//                new { user.Id, user.Ime, user.Email }
//            );
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }
//    }

//    // ================= PUTOVANJA =================
//    public async Task<List<Putovanje>> GetPutovanjaAsync()
//    {
//        var list = new List<Putovanje>();
//        var session = GetSession();

//        try
//        {
//            var cursor = await session.RunAsync("MATCH (p:Putovanje) RETURN p");
//            await cursor.ForEachAsync(record =>
//            {
//                var node = record["p"].As<INode>();
//                list.Add(new Putovanje
//                {
//                    Id = node.Properties.ContainsKey("id") ? node.Properties["id"].As<string>() : "",
//                    Naziv = node.Properties.ContainsKey("naziv") ? node.Properties["naziv"].As<string>() : "",
//                    DatumPocetka = node.Properties.ContainsKey("datumPocetka") ? DateTime.Parse(node.Properties["datumPocetka"].As<string>()) : null,
//                    DatumZavrsetka = node.Properties.ContainsKey("datumZavrsetka") ? DateTime.Parse(node.Properties["datumZavrsetka"].As<string>()) : null
//                });
//            });
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }

//        return list;
//    }

//    public async Task CreatePutovanjeAsync(Putovanje putovanje)
//    {
//        var session = GetSession();
//        try
//        {
//            await session.RunAsync(
//                "CREATE (p:Putovanje {id:$id, naziv:$naziv, datumPocetka:$datumPocetka, datumZavrsetka:$datumZavrsetka})",
//                new
//                {
//                    putovanje.Id,
//                    putovanje.Naziv,
//                    datumPocetka = putovanje.DatumPocetka?.ToString("yyyy-MM-dd"),
//                    datumZavrsetka = putovanje.DatumZavrsetka?.ToString("yyyy-MM-dd")
//                }
//            );
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }
//    }

//    // ================= DESTINACIJE =================
//    public async Task<List<Destinacija>> GetDestinacijeAsync()
//    {
//        var list = new List<Destinacija>();
//        var session = GetSession();

//        try
//        {
//            var cursor = await session.RunAsync("MATCH (d:Destinacija) RETURN d");
//            await cursor.ForEachAsync(record =>
//            {
//                var node = record["d"].As<INode>();
//                list.Add(new Destinacija
//                {
//                    Id = node.Properties.ContainsKey("id") ? node.Properties["id"].As<string>() : "",
//                    Ime = node.Properties.ContainsKey("ime") ? node.Properties["ime"].As<string>() : "",
//                    Drzava = node.Properties.ContainsKey("drzava") ? node.Properties["drzava"].As<string>() : "",
//                    Opis = node.Properties.ContainsKey("opis") ? node.Properties["opis"].As<string>() : ""
//                });
//            });
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }

//        return list;
//    }

//    public async Task CreateDestinacijaAsync(Destinacija destinacija)
//    {
//        var session = GetSession();
//        try
//        {
//            await session.RunAsync(
//                "CREATE (d:Destinacija {id:$id, ime:$ime, drzava:$drzava, opis:$opis})",
//                new { destinacija.Id, destinacija.Ime, destinacija.Drzava, destinacija.Opis }
//            );
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }
//    }

//    // ================= ATRAKCIJE =================
//    public async Task<List<Atrakcija>> GetAtrakcijeAsync()
//    {
//        var list = new List<Atrakcija>();
//        var session = GetSession();

//        try
//        {
//            var cursor = await session.RunAsync("MATCH (a:Atrakcija) RETURN a");
//            await cursor.ForEachAsync(record =>
//            {
//                var node = record["a"].As<INode>();
//                list.Add(new Atrakcija
//                {
//                    Id = node.Properties.ContainsKey("id") ? node.Properties["id"].As<string>() : "",
//                    Ime = node.Properties.ContainsKey("ime") ? node.Properties["ime"].As<string>() : "",
//                    Tip = node.Properties.ContainsKey("tip") ? node.Properties["tip"].As<string>() : ""
//                });
//            });
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }

//        return list;
//    }

//    public async Task CreateAtrakcijaAsync(Atrakcija atrakcija)
//    {
//        var session = GetSession();
//        try
//        {
//            await session.RunAsync(
//                "CREATE (a:Atrakcija {id:$id, ime:$ime, tip:$tip})",
//                new { atrakcija.Id, atrakcija.Ime, atrakcija.Tip }
//            );
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }
//    }

//    // ================= AKTIVNOSTI =================
//    public async Task<List<Aktivnost>> GetAktivnostiAsync()
//    {
//        var list = new List<Aktivnost>();
//        var session = GetSession();

//        try
//        {
//            var cursor = await session.RunAsync("MATCH (ak:Aktivnost) RETURN ak");
//            await cursor.ForEachAsync(record =>
//            {
//                var node = record["ak"].As<INode>();
//                list.Add(new Aktivnost
//                {
//                    Id = node.Properties.ContainsKey("id") ? node.Properties["id"].As<System.Guid>() : null,//vrv da se ispravi gore sa catch blok
//                    Ime = node.Properties.ContainsKey("ime") ? node.Properties["ime"].As<string>() : "",
//                    Kategorija = node.Properties.ContainsKey("kategorija") ? node.Properties["kategorija"].As<string>() : ""
//                }); ;
//            });
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }

//        return list;
//    }

//    public async Task CreateAktivnostAsync(Aktivnost aktivnost)
//    {
//        var session = GetSession();
//        try
//        {
//            await session.RunAsync(
//                "CREATE (ak:Aktivnost {id:$id, ime:$ime, kategorija:$kategorija})",
//                new { aktivnost.Id, aktivnost.Ime, aktivnost.Kategorija }
//            );
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }
//    }

//    // ================= KOMENTARI =================
//    public async Task<List<Komentar>> GetKomentariAsync()
//    {
//        var list = new List<Komentar>();
//        var session = GetSession();

//        try
//        {
//            var cursor = await session.RunAsync("MATCH (k:Komentar) RETURN k");
//            await cursor.ForEachAsync(record =>
//            {
//                var node = record["k"].As<INode>();
//                list.Add(new Komentar
//                {
//                    Id = node.Properties.ContainsKey("id") ? node.Properties["id"].As<string>() : "",
//                    Tekst = node.Properties.ContainsKey("tekst") ? node.Properties["tekst"].As<string>() : "",
//                    Ocena = node.Properties.ContainsKey("ocena") ? (int)node.Properties["ocena"].As<long>() : 0,
//                    UserId = node.Properties.ContainsKey("userId") ? node.Properties["userId"].As<string>() : "",
//                    AtrakcijaId = node.Properties.ContainsKey("atrakcijaId") ? node.Properties["atrakcijaId"].As<string>() : ""
//                });
//            });
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }

//        return list;
//    }

//    public async Task CreateKomentarAsync(Komentar komentar)
//    {
//        var session = GetSession();
//        try
//        {
//            await session.RunAsync(
//                "CREATE (k:Komentar {id:$id, tekst:$tekst, ocena:$ocena, userId:$userId, atrakcijaId:$atrakcijaId})",
//                new { komentar.Id, komentar.Tekst, komentar.Ocena, komentar.UserId, komentar.AtrakcijaId }
//            );
//        }
//        finally
//        {
//            await session.CloseAsync();
//        }
//    }
//}
