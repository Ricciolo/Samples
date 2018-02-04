using Muuvis.Catalog.Cqrs;
using Muuvis.Catalog.Cqrs.Commands;
using Muuvis.Catalog.Cqrs.Events;


// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
    public static class CqrsExtensions
    {
        public static ICqrsConfigurer AddCatalogQueue(this ICqrsConfigurer configurer)
        {
            configurer.AddQueue(Queues.Catalog.QueueName);

            return configurer;
        }

        public static ICqrsConfigurer AddCatalogCommandsRoute(this ICqrsConfigurer configurer)
        {
            configurer.AddCommandsRouteFromAssemblyOfType<AddMovieCommand>(Queues.Catalog.QueueName);

            return configurer;
        }

        public static ICqrsConfigurer AddCatalogEventsRoute(this ICqrsConfigurer configurer)
        {
            configurer.AddEventsRouteFromAssemblyOfType<MovieAddedEvent>(Queues.Catalog.EventsQueueName);

            return configurer;
        }
    }
}
