using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Muuvis.DataAccessObject;
using Muuvis.DomainModel;
using Muuvis.Repository;
using Muuvis.Taste.WebApi.Models.Suggestion;
using Rebus.Bus;

namespace Muuvis.Taste.WebApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SuggestionController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IRepository<DomainModel.Suggestion> _suggestionRepository;
        private readonly IDataAccessObject<ReadModel.Suggestion> _suggestionDataAccessObject;
        private readonly IConfigurationProvider _webApiMapper;

        public SuggestionController(WebApiMapper webApiMapper,
            IBus bus,
            IRepository<DomainModel.Suggestion> suggestionRepository,
            IDataAccessObject<ReadModel.Suggestion> suggestionDataAccessObject)
        {
            _bus = bus;
            _suggestionRepository = suggestionRepository;
            _suggestionDataAccessObject = suggestionDataAccessObject;
            _webApiMapper = webApiMapper.Mapper.ConfigurationProvider;
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<GetApiModel> Get()
        {
            return _suggestionDataAccessObject
                .ProjectTo<GetApiModel>(_webApiMapper);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSingleApiSuggestion>> Get(string id)
        {
            DomainModel.Suggestion model = await _suggestionRepository.GetAsync(id);

            if (model == null) return NotFound();

            var result = new GetSingleApiSuggestion(model);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Suggestion suggestion)
        {
            string id = IdGenerator.New();

            await _bus.Send(suggestion.GetCommand(id));

            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, Suggestion suggestion)
        {
            await _bus.Send(suggestion.GetCommand(id));
        }

     
    }
}