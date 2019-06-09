using Muuvis.Catalog.Cqrs.Commands.Movie;

// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
	public static class CqrsExtensions
	{
		public static ICqrsConfigurer UseCatalogInMemoryQueue(this ICqrsConfigurer configurer)
		{
			return configurer.UseInMemoryQueue("Catalog");
		}

		public static ICqrsConfigurer UseCatalogRabbitQueue(this ICqrsConfigurer configurer)
		{
			return configurer.UseRabbitQueue("Catalog");
		}

        public static ICqrsConfigurer UseCatalogAzureServiceBus(this ICqrsConfigurer configurer)
        {
            return configurer.UseAzureServiceBus("Catalog");
        }

        public static ICqrsConfigurer AddCatalogCommandsRoute(this ICqrsConfigurer configurer)
		{
			configurer.AddCommandsRouteFromAssemblyOfType<AddOrUpdateMovieCommand>("Catalog");
			return configurer;
		}

	}
}
