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
    public class PutovanjeController : ControllerBase
    {
        private readonly IGraphClient _client;

        public PutovanjeController(IGraphClient client)
        {
            _client = client;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var putovanja = await _client.Cypher
                .Match("(p:Putovanje)")
                .Return(p => p.As<Putovanje>())
                .ResultsAsync;

            return Ok(putovanja.ToList());
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var putovanje = await _client.Cypher
                .Match("(p:Putovanje)")
                .Where((Putovanje p) => p.guid == id)
                .Return(p => p.As<Putovanje>())
                .ResultsAsync;

            var result = putovanje.SingleOrDefault();
            return result != null ? Ok(result) : NotFound($"Putovanje sa ID {id} nije pronađeno.");
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Putovanje novoPutovanje)
        {
            await _client.Cypher
                .Create("(p:Putovanje $novoPutovanje)")
                .WithParam("novoPutovanje", novoPutovanje)
                .ExecuteWithoutResultsAsync();

            return Ok("Putovanje uspešno dodato.");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Putovanje izmenjenoPutovanje)
        {
            await _client.Cypher
                .Match("(p:Putovanje)")
                .Where((Putovanje p) => p.guid == id)
                .Set("p = $izmenjenoPutovanje")
                .WithParam("izmenjenoPutovanje", izmenjenoPutovanje)
                .ExecuteWithoutResultsAsync();

            return Ok("Putovanje uspešno izmenjeno.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _client.Cypher
                .Match("(p:Putovanje)")
                .Where((Putovanje p) => p.guid == id)
                .DetachDelete("p")
                .ExecuteWithoutResultsAsync();

            return Ok("Putovanje uspešno obrisano.");
        }
    }
}
