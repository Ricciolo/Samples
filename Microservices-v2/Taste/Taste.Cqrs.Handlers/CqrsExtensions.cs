using System;
using System.Collections.Generic;
using System.Text;
using Muuvis.Catalog.Cqrs;
using Muuvis.Cqrs.Messaging.Events;
using Muuvis.Taste.Cqrs.Handlers;

// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
	public static class CqrsExtensions
	{
		public static ICqrsConfigurer AddTasteHandlers(this ICqrsConfigurer configurer)
		{
			configurer.AddHandlersFromAssemblyOfType<SuggestionHandler>();
            return configurer;
		}

        public static void AddTasteSubscriptions(this ICqrsConfigurer configurer)
        {
            configurer.Subscribe<EntityAddedEvent<MovieType>>();
        }
    }
}
