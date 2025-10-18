using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using putovanjeApp1.Models;


namespace putovanjeApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtrakcijaController : ControllerBase
    {
        private readonly IGraphClient _client;

        public AtrakcijaController(IGraphClient client)
        {
            _client = client;
        }

        // 🟢 GET: api/atrakcija
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _client.Cypher
                .Match("(n:Atrakcija)")
                .Return(n => n.As<Atrakcija>())
                .ResultsAsync; 

            return Ok(lista.ToList());
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var atrakcija = await _client.Cypher
                .Match("(n:Atrakcija)")
                .Where((Atrakcija n) => n.guid == id)
                .Return(n => n.As<Atrakcija>())
                .ResultsAsync;

            var result = atrakcija.SingleOrDefault();
            return result != null ? Ok(result) : NotFound($"Atrakcija sa ID {id} nije pronađena.");
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Atrakcija novaAtrakcija)
        {
            await _client.Cypher
                .Create("(n:Atrakcija $novaAtrakcija)")
                .WithParam("novaAtrakcija", novaAtrakcija)
                .ExecuteWithoutResultsAsync();

            return Ok("Atrakcija uspešno dodata.");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Atrakcija izmenjenaAtrakcija)
        {
            await _client.Cypher
                .Match("(n:Atrakcija)")
                .Where((Atrakcija n) => n.guid == id)
                .Set("n = $izmenjenaAtrakcija")
                .WithParam("izmenjenaAtrakcija", izmenjenaAtrakcija)
                .ExecuteWithoutResultsAsync();

            return Ok("Atrakcija uspešno izmenjena.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _client.Cypher
                .Match("(n:Atrakcija)")
                .Where((Atrakcija n) => n.guid == id)
                .DetachDelete("n")
                .ExecuteWithoutResultsAsync();

            return Ok("Atrakcija uspešno obrisana.");
        }
    }
}

