using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using putovanjeApp1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putovanjeApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IGraphClient _client;

        public ReviewController(IGraphClient client)
        {
            _client = client;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var komentari = await _client.Cypher
                .Match("(k:Komentar)")
                .Return(k => k.As<Review>())
                .ResultsAsync;

            return Ok(komentari.ToList());
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var komentari = await _client.Cypher
                .Match("(k:Komentar)")
                .Where((Review k) => k.guid == id)
                .Return(k => k.As<Review>())
                .ResultsAsync;

            var result = komentari.SingleOrDefault();
            return result != null ? Ok(result) : NotFound($"Komentar sa ID {id} nije pronađen.");
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review noviKomentar)
        {
            await _client.Cypher
                .Create("(k:Komentar $noviKomentar)")
                .WithParam("noviKomentar", noviKomentar)
                .ExecuteWithoutResultsAsync();

            return Ok("Komentar uspešno dodat.");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Review izmenjeniKomentar)
        {
            await _client.Cypher
                .Match("(k:Komentar)")
                .Where((Review k) => k.guid == id)
                .Set("k = $izmenjeniKomentar")
                .WithParam("izmenjeniKomentar", izmenjeniKomentar)
                .ExecuteWithoutResultsAsync();

            return Ok("Komentar uspešno izmenjen.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _client.Cypher
                .Match("(k:Komentar)")
                .Where((Review k) => k.guid == id)
                .DetachDelete("k")
                .ExecuteWithoutResultsAsync();

            return Ok("Komentar uspešno obrisan.");
        }
    }
}
