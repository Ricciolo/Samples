using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Muuvis.Cqrs.Messaging.Commands;
using Muuvis.Cqrs.Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Persistence.InMem;
using Rebus.Routing;
using Rebus.Routing.TypeBased;
using Rebus.Sagas;
using Rebus.Sagas.Exclusive;
using Rebus.ServiceProvider;
using Rebus.Subscriptions;
using Rebus.Timeouts;
using Rebus.Transport;
using Rebus.Transport.InMem;

namespace Muuvis.Cqrs
{
	internal class CqrsConfigurer : ICqrsConfigurer
	{
		private readonly List<Action<StandardConfigurer<IRouter>>> _routerActions = new List<Action<StandardConfigurer<IRouter>>>();

		private readonly List<Type> _subscriptions = new List<Type>();
		private Action<StandardConfigurer<ISubscriptionStorage>> _subscriptionAction;
		private Action<StandardConfigurer<ITimeoutManager>> _timeoutAction;
		private Action<StandardConfigurer<ISagaStorage>> _sagaAction;
		private Action<StandardConfigurer<ITransport>> _transportAction;
		private TypeBasedRouterConfigurationExtensions.TypeBasedRouterConfigurationBuilder _typeBasedRouterConfigurationBuilder;

		public CqrsConfigurer(IServiceCollection services)
		{
			Services = services;
		}

		public IServiceCollection Services { get; set; }

		public IServiceProvider ServiceProvider { get; set; }

		public CqrsOptions Options => ServiceProvider.GetRequiredService<IOptions<CqrsOptions>>().Value;

		public ICqrsConfigurer AddCommandsRouteFromAssemblyOfType<T>(string queueName)
			where T : ICommand
		{
			_routerActions.Add(r => MapAssemblyOf<T, ICommand>(r, queueName));

			return this;
		}

		//public ICqrsConfigurer AddEventsRouteFromAssemblyOfType<T>(string queueName)
		//    where T : IEvent
		//{
		//    _routerActions.Add(r => MapAssemblyOf<T, IEvent>(r, queueName));

		//    return this;
		//}

		public ICqrsConfigurer UseInMemoryQueue(string queueName)
		{
			_transportAction = t => t.UseInMemoryTransport(ServiceProvider.GetRequiredService<InMemNetwork>(), queueName);
			_subscriptionAction = s => s.StoreInMemory(ServiceProvider.GetRequiredService<InMemorySubscriberStore>());
			_timeoutAction = s => s.StoreInMemory();
            _sagaAction = s =>
            {
                s.StoreInMemory();
                s.EnforceExclusiveAccess();
            };

			return this;
		}

		public ICqrsConfigurer UseRabbitQueue(string queueName)
		{
			_transportAction = t => t.UseRabbitMq(Options.RabbitMqConnectionString, queueName);
			_timeoutAction = s => s.StoreInSqlServer(Options.SqlServerConnectionString, "MessageTimeouts");
			_sagaAction = s =>
			{
				s.StoreInSqlServer(Options.SqlServerConnectionString, "Sagas", "SagasIndex");
				s.EnforceExclusiveAccess();
			};

			return this;
		}

        public ICqrsConfigurer UseAzureServiceBus(string queueName)
        {
            _transportAction = t => t.UseAzureServiceBus(Options.AzureServiceBusConnectionString, queueName);
            //_timeoutAction = s => s.StoreInSqlServer(Options.SqlServerConnectionString, "MessageTimeouts");
            _sagaAction = s =>
            {
                s.StoreInSqlServer(Options.SqlServerConnectionString, "Sagas", "SagasIndex");
                s.EnforceExclusiveAccess();
            };

            return this;
        }

        public ICqrsConfigurer AddHandlersFromAssemblyOfType<T>() where T : IHandleMessages
		{
			Services.AutoRegisterHandlersFromAssemblyOf<T>();

			return this;
		}

		public ICqrsConfigurer Subscribe<T>() where T : IEvent
		{
			if (typeof(T).IsInterface) throw new NotSupportedException("Cannot subscribe to interface type. You must specificy an object");
			_subscriptions.Add(typeof(T));

			return this;
		}

		public void ApplyActions(StandardConfigurer<IRouter> routerConfigurer)
		{
			foreach (Action<StandardConfigurer<IRouter>> routerAction in _routerActions) routerAction(routerConfigurer);
		}

		public void ApplyActions(StandardConfigurer<ITimeoutManager> timeoutConfigurer)
		{
			_timeoutAction?.Invoke(timeoutConfigurer);
		}

		public void ApplyActions(StandardConfigurer<ISagaStorage> sagaConfigurer)
		{
			_sagaAction?.Invoke(sagaConfigurer);
		}

		public void ApplyActions(StandardConfigurer<ISubscriptionStorage> subscriptiConfigurer)
		{
			_subscriptionAction?.Invoke(subscriptiConfigurer);
		}

		public void ApplyActions(StandardConfigurer<ITransport> transportConfigurer)
		{
			_transportAction?.Invoke(transportConfigurer);
		}

		private void MapAssemblyOf<T, TInterface>(StandardConfigurer<IRouter> configurer, string queueName)
		{
			_typeBasedRouterConfigurationBuilder = _typeBasedRouterConfigurationBuilder ?? configurer.TypeBased();

			foreach (Type type in typeof(T).Assembly.GetTypes().Where(t => typeof(TInterface).IsAssignableFrom(t))) _typeBasedRouterConfigurationBuilder.Map(type, queueName);
		}

		public async Task ApplySubscriptionsAsync(IBus bus)
		{
			foreach (Type subscription in _subscriptions) await bus.Subscribe(subscription);
		}
	}
}