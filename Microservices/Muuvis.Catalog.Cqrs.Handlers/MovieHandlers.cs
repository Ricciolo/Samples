using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.DomainModel;
using Muuvis.Common;
using Muuvis.DataAccessObject;
using Muuvis.Repository;
using Rebus.Handlers;

namespace Muuvis.Catalog.Cqrs.Handlers
{
    public class MovieHandlers : IHandleMessages<AddMovieCommand>
    {
        private readonly ILogger<MovieHandlers> _logger;
        private readonly IUserAccessor _userAccessor;
        private readonly IRepository<Movie> _repository;
        private readonly IDataAccessObject<ReadModel.Movie> _dataAccessObject;

        public MovieHandlers(
            ILogger<MovieHandlers> logger,
            IUserAccessor userAccessor,
            IRepository<Movie> repository,
            IDataAccessObject<ReadModel.Movie> dataAccessObject)
        {
            _logger = logger;
            _userAccessor = userAccessor;
            _repository = repository;
            _dataAccessObject = dataAccessObject;
        }

        public async Task Handle(AddMovieCommand message)
        {
            if (await _dataAccessObject.AnyAsync(m => m.Id == message.Id)) return;

            _logger.LogInformation("Add movie {title} requested by {user}", message.Title, _userAccessor.User.Identity.Name);

            var movie = new Movie(message.Id, message.Title)
            {
                Year = message.Year
            };
            await _repository.AddAsync(movie);
        }
    }
}
