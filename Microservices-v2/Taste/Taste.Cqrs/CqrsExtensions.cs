using Muuvis.Taste.Cqrs.Commands.Suggestion;

// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
	public static class CqrsExtensions
	{
		public static ICqrsConfigurer UseTasteInMemoryQueue(this ICqrsConfigurer configurer)
		{
			return configurer.UseInMemoryQueue("Taste");
		}

		public static ICqrsConfigurer UseTasteRabbitQueue(this ICqrsConfigurer configurer)
		{
			return configurer.UseRabbitQueue("Taste");
		}

        public static ICqrsConfigurer UseTasteAzureServiceBus(this ICqrsConfigurer configurer)
        {
            return configurer.UseAzureServiceBus("Taste");
        }

        public static ICqrsConfigurer AddTasteCommandsRoute(this ICqrsConfigurer configurer)
		{
			configurer.AddCommandsRouteFromAssemblyOfType<AddOrUpdateSuggestionCommand>("Taste");
			return configurer;
		}

	}
}
