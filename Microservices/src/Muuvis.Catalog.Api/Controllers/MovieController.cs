using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Muuvis.Catalog.Api.Models.Movie;
using Muuvis.Catalog.Cqrs;
using Muuvis.Catalog.Cqrs.Commands;
using Muuvis.Catalog.Cqrs.Events;
using Muuvis.Common;
using Muuvis.Cqrs.Rebus;
using Rebus;
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
            // Hack: just for testing claims into the command handler
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

            //await bus.Send(command);
            string movieId = await bus.SendRequest<string>(command);

            return CreatedAtAction("Get", new { id = movieId });
        }

    }
}
