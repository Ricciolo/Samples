using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.Cqrs.Commands;
using Muuvis.Catalog.Cqrs.Events;
using Muuvis.Catalog.DomainModel;
using Muuvis.Common;
using Muuvis.DataAccessObject;
using Muuvis.Repository;
using Rebus;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.TransactionScopes;

namespace Muuvis.Catalog.Cqrs.Handlers
{
    public class MovieHandlers : IHandleMessages<AddMovieCommand>, IHandleMessages<MovieAddedEvent>
    {
        private readonly ILogger<MovieHandlers> _logger;
        private readonly IBus _bus;
        private readonly IUserAccessor _userAccessor;
        private readonly IRepository<Movie> _repository;
        private readonly IDataAccessObject<ReadModel.Movie> _dataAccessObject;

        public MovieHandlers(
            ILogger<MovieHandlers> logger,
            IBus bus,
            IUserAccessor userAccessor,
            IRepository<Movie> repository,
            IDataAccessObject<ReadModel.Movie> dataAccessObject)
        {
            _logger = logger;
            _bus = bus;
            _userAccessor = userAccessor;
            _repository = repository;
            _dataAccessObject = dataAccessObject;
        }

        public async Task Handle(AddMovieCommand command)
        {
            if (await _dataAccessObject.AnyAsync(m => m.Id == command.Id))
            {
                _logger.LogWarning("Movie {title} already added", command.Title);
                return;
            }

            _logger.LogInformation("Add movie {title} requested by {user}", command.Title, _userAccessor.User?.Identity.Name);

            //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    transaction.EnlistRebus();

                var movie = new Movie(command.Id, command.Title)
                {
                    Year = command.Year
                };
                await _repository.AddAsync(movie);

                await _bus.Publish(new MovieAddedEvent { Id = command.Id });

            //    transaction.Complete();
            //}

            await _bus.Reply(command.Id);
        }

        public Task Handle(MovieAddedEvent message)
        {
            // For testing purpose only
            return Task.CompletedTask;
        }
    }

}
