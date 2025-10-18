using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using putovanjeApp1.Models;


namespace putovanjeApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestinacijaController : ControllerBase
    {
        private readonly IGraphClient _client;

        public DestinacijaController(IGraphClient client)
        {
            _client = client;
        }

        // 🟢 GET: api/destinacija
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var destinacije = await _client.Cypher
                .Match("(d:Destinacija)")
                .Return(d => d.As<Destinacija>())
                .ResultsAsync;

            return Ok(destinacije.ToList());
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var destinacija = await _client.Cypher
                .Match("(d:Destinacija)")
                .Where((Destinacija d) => d.guid == id)
                .Return(d => d.As<Destinacija>())
                .ResultsAsync;

            var result = destinacija.SingleOrDefault();
            return result != null ? Ok(result) : NotFound($"Destinacija sa ID {id} nije pronađena.");
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Destinacija novaDestinacija)
        {
            await _client.Cypher
                .Create("(d:Destinacija $novaDestinacija)")
                .WithParam("novaDestinacija", novaDestinacija)
                .ExecuteWithoutResultsAsync();

            return Ok("Destinacija uspešno dodata.");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Destinacija izmenjenaDestinacija)
        {
            await _client.Cypher
                .Match("(d:Destinacija)")
                .Where((Destinacija d) => d.guid == id)
                .Set("d = $izmenjenaDestinacija")
                .WithParam("izmenjenaDestinacija", izmenjenaDestinacija)
                .ExecuteWithoutResultsAsync();

            return Ok("Destinacija uspešno izmenjena.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _client.Cypher
                .Match("(d:Destinacija)")
                .Where((Destinacija d) => d.guid == id)
                .DetachDelete("d")
                .ExecuteWithoutResultsAsync();

            return Ok("Destinacija uspešno obrisana.");
        }
    }
}

