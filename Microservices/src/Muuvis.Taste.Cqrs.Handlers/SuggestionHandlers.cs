using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.Cqrs.Events;
using Muuvis.Common;
using Muuvis.DataAccessObject;
using Muuvis.Repository;
using Muuvis.Taste.Cqrs.Commands;
using Muuvis.Taste.DomainModel;
using Rebus.Bus;
using Rebus.Handlers;

namespace Muuvis.Taste.Cqrs.Handlers
{
    public class SuggestionHandlers : IHandleMessages<EvaluateSuggestionCommand>, IHandleMessages<MovieAddedEvent>
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
            if (await _dataAccessObject.AnyAsync(m => m.MovieId == message.MovieId)) return;

            _logger.LogInformation("Evaluating suggestion for {movieId}", message.MovieId);

            await _repository.AddAsync(new Suggestion(message.MovieId)
            {
                Affinity = (float) Math.Round(new Random(Environment.TickCount).NextDouble(), 2)
            });
        }

        public async Task Handle(MovieAddedEvent message)
        {
            _logger.LogInformation("New movie {movieId} added", message.Id);

            await _bus.Send(new EvaluateSuggestionCommand {MovieId = message.Id});
        }
    }

}
