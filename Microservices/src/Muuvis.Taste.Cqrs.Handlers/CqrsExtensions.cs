using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Muuvis.Catalog.Cqrs;
using Muuvis.Catalog.Cqrs.Events;
using Muuvis.Cqrs;
using Muuvis.Taste.Cqrs.Handlers;
using Rebus.Routing;
using Rebus.Routing.TypeBased;


// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
    public static class CqrsExtensions
    {
        public static ICqrsConfigurer AddTasteHandlers(this ICqrsConfigurer configurer)
        {
            configurer.AddHandlersFromAssemblyOfType<SuggestionHandlers>();

            return configurer;
        }

        public static void TasteSubscribe(this ICqrsConfigurer configurer)
        {
            configurer.Subscribe<MovieAddedEvent>();
        }
    }
}
