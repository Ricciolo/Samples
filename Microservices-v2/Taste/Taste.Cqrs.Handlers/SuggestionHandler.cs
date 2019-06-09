using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.Cqrs;
using Muuvis.Cqrs.Messaging.Events;
using Muuvis.DataAccessObject;
using Muuvis.DomainModel;
using Muuvis.Repository;
using Muuvis.Taste.Cqrs.Commands.Suggestion;
using Muuvis.Taste.DomainModel;
using Rebus.Bus;
using Rebus.Handlers;

namespace Muuvis.Taste.Cqrs.Handlers
{
    internal class SuggestionHandler :
        IHandleMessages<AddOrUpdateSuggestionCommand>,
        IHandleMessages<EntityAddedEvent<MovieType>>,
        IHandleMessages<EvaluateSuggestionCommand>
    {
        private readonly IBus _bus;
        private readonly ILogger<SuggestionHandler> _logger;
        private readonly IRepository<Suggestion> _suggestionRepository;
        private readonly IDataAccessObject<ReadModel.Suggestion> _suggestionsDataAccessObject;

        public SuggestionHandler(
            ILogger<SuggestionHandler> logger,
            IBus bus,
            IRepository<Suggestion> suggestionRepository,
            IDataAccessObject<ReadModel.Suggestion> suggestionsDataAccessObject
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _suggestionRepository = suggestionRepository ?? throw new ArgumentNullException(nameof(suggestionRepository));
            _suggestionsDataAccessObject = suggestionsDataAccessObject ?? throw new ArgumentNullException(nameof(suggestionsDataAccessObject));
        }

        public async Task Handle(AddOrUpdateSuggestionCommand command)
        {
            var entity = new Suggestion(command.Id, command.MovieId)
            {
                Affinity = command.Affinity
            };

            bool exists = await _suggestionsDataAccessObject.AnyAsync(d => d.Id == command.Id);

            if (exists)
            {
                await _suggestionRepository.UpdateAsync(entity);

                _logger.LogInformation("Suggestion {value} updated", command.Id);

                await _bus.Publish(command.CreateUpdatedEvent());
            }
            else
            {
                _logger.LogInformation("Suggestion {value} added", command.Id);

                await _suggestionRepository.AddAsync(entity);

                await _bus.Publish(command.CreateAddedEvent());
            }
        }

        public Task Handle(EntityAddedEvent<MovieType> message)
        {
            _logger.LogInformation("New movie {movieId} added", message.Id);

            return _bus.Send(new EvaluateSuggestionCommand(message.Id));
        }

        public async Task Handle(EvaluateSuggestionCommand message)
        {
            // Idempotency
            if (await _suggestionsDataAccessObject.AnyAsync(m => m.MovieId == message.MovieId)) return;

            _logger.LogInformation("Evaluating suggestion for {movieId}", message.MovieId);

            //if (Environment.TickCount % 2d == 0d)
            //{
            //    throw new Exception("Random error");
            //}

            await _suggestionRepository.AddAsync(new Suggestion(IdGenerator.New(), message.MovieId)
            {
                Affinity = (float)Math.Round(new Random(Environment.TickCount).NextDouble(), 2)
            });

            message.CreateCompletedEvent();
        }
    }
}