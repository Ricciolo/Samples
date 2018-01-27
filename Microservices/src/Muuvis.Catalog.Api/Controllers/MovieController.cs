using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Muuvis.Catalog.Api.Models.Movie;
using Muuvis.Catalog.Cqrs;
using Rebus.Bus;

namespace Muuvis.Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromServices] IBus bus,
            [Required, FromBody]PostModel model)
        {
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new [] {new Claim(ClaimsIdentity.DefaultNameClaimType, "test")}, "test"));

            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new AddMovieCommand
            {
                Id = NewId.Next().ToString(),
                Title = model.Title,
                Year = model.Year
            };

            await bus.Send(command);

            return CreatedAtAction("Get", new { command.Id });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
