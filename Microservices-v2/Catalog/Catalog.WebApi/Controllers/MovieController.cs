using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class MovieController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IRepository<Movie> _movieRepository;
        private readonly IDataAccessObject<MovieRead> _modelDataAccessObject;
        private readonly IConfigurationProvider _webApiMapper;

        public MovieController(WebApiMapper webApiMapper,
            IBus bus,
            IRepository<Movie> movieRepository,
            IDataAccessObject<MovieRead> modelDataAccessObject)
        {
            _bus = bus;
            _movieRepository = movieRepository;
            _modelDataAccessObject = modelDataAccessObject;
            _webApiMapper = webApiMapper.Mapper.ConfigurationProvider;
        }

        [HttpGet("originals")]
        [EnableQuery]
        public IQueryable<WebApiModels.GetApiModel> GetOriginals()
        {
            return _modelDataAccessObject
                .Where(m => m.IsOriginal)
                .ProjectTo<WebApiModels.GetApiModel>(_webApiMapper);
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<WebApiModels.GetApiModel> Get()
        {
            return _modelDataAccessObject
                .ProjectTo<WebApiModels.GetApiModel>(_webApiMapper);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WebApiModels.GetSingleApiMovie>> Get(string id)
        {
            Movie model = await _movieRepository.GetAsync(id);

            if (model == null) return NotFound();

            var result = new WebApiModels.GetSingleApiMovie(model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(WebApiModels.Movie movie)
        {
            string id = IdGenerator.New();

            await _bus.Send(movie.GetCommand(id));

            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, WebApiModels.Movie movie)
        {
            await _bus.Send(movie.GetCommand(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _bus.Send(new DeleteMovieCommand(id));

            return Ok();
        }
    }
}