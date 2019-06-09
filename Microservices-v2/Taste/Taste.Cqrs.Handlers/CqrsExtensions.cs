using System;
using System.Collections.Generic;
using System.Text;
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

	}
}
