using Muuvis.Taste.Cqrs.Commands;
//using Muuvis.Taste.Cqrs.Events;


// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
    public static class CqrsExtensions
    {
        public static ICqrsConfigurer AddTasteQueue(this ICqrsConfigurer configurer)
        {
            configurer.AddQueue(Queues.Taste.QueueName);

            return configurer;
        }

        public static ICqrsConfigurer AddTasteCommandsRoute(this ICqrsConfigurer configurer)
        {
            configurer.AddCommandsRouteFromAssemblyOfType<EvaluateSuggestionCommand>(Queues.Taste.QueueName);

            return configurer;
        }

        public static ICqrsConfigurer AddTasteEventsRoute(this ICqrsConfigurer configurer)
        {
            //configurer.AddEventsRouteFromAssemblyOfType<MovieAddedEvent>(Queues.Taste.EventsQueueName);

            return configurer;
        }
    }
}
