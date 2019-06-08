using System;
using System.Collections.Generic;
using System.Text;
using Muuvis.Catalog.Cqrs.Handlers;

// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
	public static class CqrsExtensions
	{
		public static ICqrsConfigurer AddCatalogHandlers(this ICqrsConfigurer configurer)
		{
			configurer.AddHandlersFromAssemblyOfType<MovieHandler>();
            return configurer;
		}

	}
}
