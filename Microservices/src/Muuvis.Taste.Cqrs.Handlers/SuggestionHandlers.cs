using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.Cqrs.Events;
using Muuvis.Common;
using Muuvis.Cqrs;
using Muuvis.DataAccessObject;
using Muuvis.Repository;
using Muuvis.Taste.Cqrs.Commands;
using Muuvis.Taste.Cqrs.Events;
using Muuvis.Taste.DomainModel;
using Rebus;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Retry.Simple;

namespace Muuvis.Taste.Cqrs.Handlers
{
    public class SuggestionHandlers : IHandleMessages<EvaluateSuggestionCommand>, IHandleMessages<MovieAddedEvent>, IHandleMessages<IFailed<EvaluateSuggestionCommand>>
    {
        private readonly ILogger<SuggestionHandlers> _logger;
        private readonly IBus _bus;
        private readonly IUserAccessor _userAccessor;
        private readonly IRepository<Suggestion> _repository;
        private readonly IDataAccessObject<Taste.ReadModel.Suggestion> _dataAccessObject;

        public SuggestionHandlers(
            ILogger<SuggestionHandlers> logger,
            IBus bus,
            IUserAccessor userAccessor,
            IRepository<Suggestion> repository,
            IDataAccessObject<Taste.ReadModel.Suggestion> dataAccessObject)
        {
            _logger = logger;
            _bus = bus;
            _userAccessor = userAccessor;
            _repository = repository;
            _dataAccessObject = dataAccessObject;
        }

        public async Task Handle(EvaluateSuggestionCommand message)
        {
            if (!await _dataAccessObject.AnyAsync(m => m.MovieId == message.MovieId))
            {
                _logger.LogInformation("Evaluating suggestion for {movieId}", message.MovieId);

                //if (Environment.TickCount % 2d == 0d)
                //{
                //    throw new Exception("Random error");
                //}

                await _repository.AddAsync(new Suggestion(message.MovieId)
                {
                    Affinity = (float) Math.Round(new Random(Environment.TickCount).NextDouble(), 2)
                });
            }

            await _bus.Publish(new SuggestionEvaluatedEvent {MovieId = message.MovieId});
        }

        public async Task Handle(MovieAddedEvent message)
        {
            _logger.LogInformation("New movie {movieId} added", message.MovieId);

            await _bus.Send(new EvaluateSuggestionCommand {MovieId = message.MovieId});
        }

        public Task Handle(IFailed<EvaluateSuggestionCommand> message)
        {
            return _bus.ExponentialRetry(message, 3);
        }
    }

}
