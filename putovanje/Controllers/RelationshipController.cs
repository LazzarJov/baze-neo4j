using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using putovanjeApp1.Models;
using System;
using System.Threading.Tasks;

namespace putovanjeApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationshipController : ControllerBase
    {
        private readonly IGraphClient _client;

        public RelationshipController(IGraphClient client)
        {
            _client = client;
        }

        // User PLANIRAO Putovanje
        [HttpPost("user/{userGuid}/planirao/{putovanjeGuid}")]
        public async Task<IActionResult> CreatePlanirao(Guid userGuid, Guid putovanjeGuid)
        {
            var exists = await _client.Cypher
                .Match("(u:User)-[r:PLANIRAO]->(p:Putovanje)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Putovanje p) => p.guid == putovanjeGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza PLANIRAO već postoji.");

            await _client.Cypher
                .Match("(u:User)", "(p:Putovanje)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Putovanje p) => p.guid == putovanjeGuid)
                .Create("(u)-[:PLANIRAO]->(p)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza PLANIRAO kreirana.");
        }

        // User BIO_NA Putovanje
        [HttpPost("user/{userGuid}/bio_na/{putovanjeGuid}")]
        public async Task<IActionResult> CreateBioNa(Guid userGuid, Guid putovanjeGuid)
        {
            var exists = await _client.Cypher
                .Match("(u:User)-[r:BIO_NA]->(p:Putovanje)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Putovanje p) => p.guid == putovanjeGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza BIO_NA već postoji.");

            await _client.Cypher
                .Match("(u:User)", "(p:Putovanje)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Putovanje p) => p.guid == putovanjeGuid)
                .Create("(u)-[:BIO_NA]->(p)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza BIO_NA kreirana.");
        }

        // User NAPISAO Review
        [HttpPost("user/{userGuid}/napisao/{reviewGuid}")]
        public async Task<IActionResult> CreateNapisao(Guid userGuid, Guid reviewGuid)
        {
            var exists = await _client.Cypher
                .Match("(u:User)-[r:NAPISAO]->(rev:Review)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Review rev) => rev.guid == reviewGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza NAPISAO već postoji.");

            await _client.Cypher
                .Match("(u:User)", "(rev:Review)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Review rev) => rev.guid == reviewGuid)
                .Create("(u)-[:NAPISAO]->(rev)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza NAPISAO kreirana.");
        }

        // Putovanje OBUHVATA Destinaciju
        [HttpPost("putovanje/{putovanjeGuid}/obuhvata/{destinacijaGuid}")]
        public async Task<IActionResult> CreateObuhvata(Guid putovanjeGuid, Guid destinacijaGuid)
        {
            var exists = await _client.Cypher
                .Match("(p:Putovanje)-[r:OBUHVATA]->(d:Destinacija)")
                .Where((Putovanje p) => p.guid == putovanjeGuid)
                .AndWhere((Destinacija d) => d.guid == destinacijaGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza OBUHVATA već postoji.");

            await _client.Cypher
                .Match("(p:Putovanje)", "(d:Destinacija)")
                .Where((Putovanje p) => p.guid == putovanjeGuid)
                .AndWhere((Destinacija d) => d.guid == destinacijaGuid)
                .Create("(p)-[:OBUHVATA]->(d)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza OBUHVATA kreirana.");
        }

        // Destinacija IMA_ATRAKCIJU Atrakciju
        [HttpPost("destinacija/{destinacijaGuid}/ima_atrakciju/{atrakcijaGuid}")]
        public async Task<IActionResult> CreateImaAtrakciju(Guid destinacijaGuid, Guid atrakcijaGuid)
        {
            var exists = await _client.Cypher
                .Match("(d:Destinacija)-[r:IMA_ATRAKCIJU]->(a:Atrakcija)")
                .Where((Destinacija d) => d.guid == destinacijaGuid)
                .AndWhere((Atrakcija a) => a.guid == atrakcijaGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza IMA_ATRAKCIJU već postoji.");

            await _client.Cypher
                .Match("(d:Destinacija)", "(a:Atrakcija)")
                .Where((Destinacija d) => d.guid == destinacijaGuid)
                .AndWhere((Atrakcija a) => a.guid == atrakcijaGuid)
                .Create("(d)-[:IMA_ATRAKCIJU]->(a)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza IMA_ATRAKCIJU kreirana.");
        }

        // Atrakcija NUDI Aktivnost
        [HttpPost("atrakcija/{atrakcijaGuid}/nudi/{aktivnostGuid}")]
        public async Task<IActionResult> CreateNudi(Guid atrakcijaGuid, Guid aktivnostGuid)
        {
            var exists = await _client.Cypher
                .Match("(a:Atrakcija)-[r:NUDI]->(act:Aktivnost)")
                .Where((Atrakcija a) => a.guid == atrakcijaGuid)
                .AndWhere((Aktivnost act) => act.guid == aktivnostGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza NUDI već postoji.");

            await _client.Cypher
                .Match("(a:Atrakcija)", "(act:Aktivnost)")
                .Where((Atrakcija a) => a.guid == atrakcijaGuid)
                .AndWhere((Aktivnost act) => act.guid == aktivnostGuid)
                .Create("(a)-[:NUDI]->(act)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza NUDI kreirana.");
        }

        // Review ZA Atrakciju
        [HttpPost("review/{reviewGuid}/za/{atrakcijaGuid}")]
        public async Task<IActionResult> CreateZa(Guid reviewGuid, Guid atrakcijaGuid)
        {
            var exists = await _client.Cypher
                .Match("(rev:Review)-[r:ZA]->(a:Atrakcija)")
                .Where((Review rev) => rev.guid == reviewGuid)
                .AndWhere((Atrakcija a) => a.guid == atrakcijaGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza ZA već postoji.");

            await _client.Cypher
                .Match("(rev:Review)", "(a:Atrakcija)")
                .Where((Review rev) => rev.guid == reviewGuid)
                .AndWhere((Atrakcija a) => a.guid == atrakcijaGuid)
                .Create("(rev)-[:ZA]->(a)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza ZA kreirana.");
        }


        // User VOLI Aktivnost
        [HttpPost("user/{userGuid}/voli/{aktivnostGuid}")]
        public async Task<IActionResult> CreateVoli(Guid userGuid, Guid aktivnostGuid)
        {
            var exists = await _client.Cypher
                .Match("(u:User)-[r:VOLI]->(a:Aktivnost)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Aktivnost a) => a.guid == aktivnostGuid)
                .Return(r => r.Count())
                .ResultsAsync;

            if (exists.Single() > 0)
                return BadRequest("Veza VOLI već postoji.");

            await _client.Cypher
                .Match("(u:User)", "(a:Aktivnost)")
                .Where((User u) => u.guid == userGuid)
                .AndWhere((Aktivnost a) => a.guid == aktivnostGuid)
                .Create("(u)-[:VOLI]->(a)")
                .ExecuteWithoutResultsAsync();

            return Ok("Veza VOLI kreirana.");
        }




    }
}
