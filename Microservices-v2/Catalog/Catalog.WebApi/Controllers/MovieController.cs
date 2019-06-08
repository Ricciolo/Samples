using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Muuvis.Catalog.Cqrs.Commands.Movie;
using Muuvis.Catalog.DomainModel;
using Muuvis.Catalog.ReadModel;
using Muuvis.DataAccessObject;
using Muuvis.DomainModel;
using Muuvis.Repository;
using Rebus.Bus;
using WebApiModels = Muuvis.Catalog.WebApi.Models.Movie;

namespace Muuvis.Catalog.WebApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        [HttpGet("originals")]
        [EnableQuery]
        public IQueryable<WebApiModels.GetApiModel> GetOriginals(
            [FromServices] IDataAccessObject<MovieRead> modelDataAccessObject,
            [FromServices] WebApiMapper webApiMapper)
        {
            return modelDataAccessObject
                .Where(m => m.IsOriginal)
                .ProjectTo<WebApiModels.GetApiModel>(webApiMapper.Mapper.ConfigurationProvider);
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<WebApiModels.GetApiModel> Get(
            [FromServices] IDataAccessObject<MovieRead> modelDataAccessObject,
            [FromServices] WebApiMapper webApiMapper)
        {
            return modelDataAccessObject
                .ProjectTo<WebApiModels.GetApiModel>(webApiMapper.Mapper.ConfigurationProvider);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WebApiModels.GetSingleApiMovie>> GetSingle(string id,
            [FromServices] IRepository<Movie> movieRepository
        )
        {
            Movie model = await movieRepository.GetAsync(id);

            if (model == null) return NotFound();

            var result = new WebApiModels.GetSingleApiMovie(model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            WebApiModels.Movie movie,
            [FromServices] IBus bus)
        {
            string id = IdGenerator.New();

            await bus.Send(movie.GetCommand(id));

            return Created(Url.Action(nameof(Get), new {id}), id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id,
            WebApiModels.Movie movie,
            [FromServices] IBus bus)
        {
            await bus.Send(movie.GetCommand(id));

            return Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            [FromServices] IBus bus,
            string id)
        {
            await bus.Send(new DeleteMovieCommand(id));

            return Ok();
        }
    }
}