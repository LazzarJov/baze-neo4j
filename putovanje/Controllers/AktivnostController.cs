using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using putovanjeApp1.Models;

namespace putovanjeApp1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AktivnostController : ControllerBase
{
    private readonly IGraphClient _client;

    public AktivnostController(IGraphClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _client.Cypher
            .Match("(n:Aktivnost)")
            .Return(n => n.As<Aktivnost>())
            .ResultsAsync;

        return Ok(lista);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var aktivnost = await _client.Cypher
            .Match("(n:Aktivnost)")
            .Where((Aktivnost n) => n.guid == id)
            .Return(n => n.As<Aktivnost>())
            .ResultsAsync;

        var result = aktivnost.SingleOrDefault();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Aktivnost novaAktivnost)
    {
        await _client.Cypher
                .Create("(n:Aktivnost $novaAktivnost)")
                .WithParam("novaAktivnost", novaAktivnost)
                .ExecuteWithoutResultsAsync();

        return Ok("Aktivnost uspešno dodata.");
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Aktivnost izmenjenaAktivnost)
    {
        await _client.Cypher
            .Match("(n:Aktivnost)")
            .Where((Aktivnost n) => n.guid == id)
            .Set("n = $izmenjenaAktivnost")
            .WithParam("izmenjenaAktivnost", izmenjenaAktivnost)
            .ExecuteWithoutResultsAsync();

        return Ok("Aktivnost uspešno izmenjena.");
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _client.Cypher
            .Match("(n:Aktivnost)")
            .Where((Aktivnost n) => n.guid == id)
            .DetachDelete("n")
            .ExecuteWithoutResultsAsync();

        return Ok("Aktivnost uspešno obrisana.");
    }








}


