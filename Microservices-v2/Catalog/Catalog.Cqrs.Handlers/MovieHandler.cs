using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.Cqrs.Commands.Movie;
using Muuvis.Catalog.DomainModel;
using Muuvis.Catalog.ReadModel;
using Muuvis.Cqrs.Messaging.Events;
using Muuvis.DataAccessObject;
using Muuvis.DomainModel;
using Muuvis.Repository;
using Rebus.Bus;
using Rebus.Handlers;

namespace Muuvis.Catalog.Cqrs.Handlers
{
    internal class MovieHandler : IHandleMessages<AddOrUpdateMovieCommand>,
        IHandleMessages<DeleteMovieCommand>
    {
        private readonly IBus _bus;
        private readonly ILogger<MovieHandler> _logger;
        private readonly IDataAccessObject<MovieRead> _movieDataAccessObject;
        private readonly IRepository<Movie> _movieRepository;

        public MovieHandler(
            ILogger<MovieHandler> logger,
            IBus bus,
            IRepository<Movie> movieRepository,
            IDataAccessObject<MovieRead> movieDataAccessObject
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _movieDataAccessObject = movieDataAccessObject;
        }

        public async Task Handle(AddOrUpdateMovieCommand command)
        {
            // Validate title uniqueness
            if (await _movieDataAccessObject.AnyAsync(m => m.Title == command.OriginalTitle))
            {
                _logger.LogError("A movie with the title {title} already exists", command.OriginalTitle);

                await _bus.Publish(command.CreateValidationEvent(new ValidationResult(409, $"A movie with the title {command.OriginalTitle} already exists")));

                return;
            }

            var entity = new Movie(command.Id, command.OriginalCulture, command.OriginalTitle)
            {
                Year = command.Year
            };
            foreach (var pair in command.Translation)
            {
                entity.Translation.Add(pair.Key, pair.Value);
            }

            bool exists = await _movieDataAccessObject.AnyAsync(e => e.Id == command.Id);

            if (exists)
            {
                await _movieRepository.UpdateAsync(entity);

                _logger.LogInformation("Movie {value} updated", command.Id);

                await _bus.Publish(command.CreateUpdatedEvent());
            }
            else
            {
                _logger.LogInformation("Movie {value} added", command.Id);

                await _movieRepository.AddAsync(entity);

                await _bus.Publish(command.CreateAddedEvent());
            }
        }

        public async Task Handle(DeleteMovieCommand command)
        {
            Movie entity = await _movieRepository.GetAsync(command.Id);

            if (entity == null || entity.IsDeleted)
            {
                _logger.LogWarning("Movie with id {id} is already deleted", command.Id);
            }
            else
            {
                entity.IsDeleted = true;

                await _movieRepository.UpdateAsync(entity);

                await _bus.Publish(command.CreateUpdatedEvent());

                _logger.LogInformation("Movie with id {id} has been deleted", command.Id);
            }
        }
    }
}