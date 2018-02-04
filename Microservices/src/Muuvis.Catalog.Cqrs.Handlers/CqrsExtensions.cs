using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Muuvis.Catalog.Cqrs;
using Muuvis.Catalog.Cqrs.Events;
using Muuvis.Catalog.Cqrs.Handlers;
using Muuvis.Cqrs;
using Muuvis.Taste.Cqrs.Events;
using Rebus.Bus;
using Rebus.Routing;
using Rebus.Routing.TypeBased;


// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
    public static class CqrsExtensions
    {
        public static ICqrsConfigurer AddCatalogHandlers(this ICqrsConfigurer configurer)
        {
            configurer.AddHandlersFromAssemblyOfType<MovieHandlers>();

            return configurer;
        }

        public static void CatalogSubscribe(this ICqrsConfigurer configurer)
        {
            configurer.Subscribe<MovieAddedEvent>();
            configurer.Subscribe<SuggestionEvaluatedEvent>();
        }
    }
}
