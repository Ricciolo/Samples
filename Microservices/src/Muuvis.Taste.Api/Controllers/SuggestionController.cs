using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Muuvis.DataAccessObject;
using Muuvis.Taste.Api.Models.Suggestion;
using Rebus.Bus;

namespace Muuvis.Taste.Api.Controllers
{
    [Route("[controller]")]
    public class SuggestionController : Controller
    {
        // GET suggestion
        [HttpGet]
        public async Task<IEnumerable<GetModel>> Get([FromServices]IDataAccessObject<ReadModel.Suggestion> suggestionsDataAccessObject)
        {
            return await suggestionsDataAccessObject.Select(d => new GetModel
            {
                Id = d.MovieId,
                Affinity = d.Affinity,
            }).ToArrayAsync();
        }

        // GET suggestion/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromServices]IDataAccessObject<ReadModel.Suggestion> suggestionsDataAccessObject, string id)
        {
            GetModel result = await suggestionsDataAccessObject.Where(d => d.MovieId == id)
                .Select(d => new GetModel
                {
                    Id = d.MovieId,
                    Affinity = d.Affinity,
                }).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
