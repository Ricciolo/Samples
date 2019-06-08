using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Muuvis;
using Muuvis.Cqrs;
using Muuvis.Cqrs.Json;
using Muuvis.Cqrs.Messaging;
using Muuvis.Cqrs.Rebus;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Persistence.InMem;
using Rebus.Retry.Simple;
using Rebus.Serialization.Json;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;
using Serilog;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extensions for configuring service bus
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        ///     Adds service bus memory implementations copying that from the provider passed
        /// </summary>
        /// <param name="services"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBusDependencies(this IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton(c => provider.GetRequiredService<InMemNetwork>());
            services.AddSingleton(c => provider.GetRequiredService<InMemorySubscriberStore>());

            return services;
        }

        /// <summary>
        ///     Adds all bus services allowing the configuration through <paramref name="configurerAction" />
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurerAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBus(this IServiceCollection services, Action<ICqrsConfigurer> configurerAction)
        {
            var configurer = GetCqrsConfigurer(services);
            configurerAction(configurer);

            // Add default network and store inmemory implementation, if not provided
            services.TryAddSingleton<InMemNetwork>();
            services.TryAddSingleton<InMemorySubscriberStore>();
            services.TryAddSingleton<MessagesWaiter>();
            services.TryAddSingleton<IMessagesWaiter>(p => p.GetRequiredService<MessagesWaiter>());
            services.TryAddScoped<IMessagesCatcher, MessagesCatcher>();
            services.TryAddSingleton(c => (IHandleMessages<IMessage>) c.GetRequiredService<MessagesWaiter>());

            services.AddRebus(r => r);
            services.RemoveAll<NetCoreServiceProviderContainerAdapter>();
            services.RemoveAll<IBus>();

            services.AddSingleton<NetCoreServiceProviderContainerAdapterEx>();
            services.AddSingleton(provider =>
            {
                RebusConfigurer c = Configure.With(provider.GetRequiredService<NetCoreServiceProviderContainerAdapterEx>());
                ConfigureRebus(c, provider);
                return c.Start();
            });

            services.AddSingleton<IHostedService, RebusHostedService>(r => new RebusHostedService(r, configurer));

            return services;

            RebusConfigurer ConfigureRebus(RebusConfigurer configure, IServiceProvider provider)
            {
                var options = provider.GetRequiredService<IOptions<CqrsOptions>>();
                configurer.ServiceProvider = provider;
                var serilog = provider.GetService<ILogger>();

                return configure.Serialization(s => s.UseNewtonsoftJson(GetJsonSettings()))
                    .Options(o =>
                    {
                        o.SetNumberOfWorkers(options.Value.ServiceBusWorkers);
                        o.SetMaxParallelism(options.Value.ServiceBusWorkers);
                        o.ApplyServiceProvider(provider);
                        o.AutoSetMessageId();
                        o.CatchMessagesSent();
                        o.HandleCommandsEvents(provider);
                        o.SimpleRetryStrategy(maxDeliveryAttempts: 2, secondLevelRetriesEnabled: true);
                    })
                    .Timeouts(configurer.ApplyActions)
                    .Sagas(configurer.ApplyActions)
                    .Logging(l =>
                    {
                        if (serilog != null) l.Serilog(serilog);
                    })
                    .Subscriptions(configurer.ApplyActions)
                    .Transport(configurer.ApplyActions)
                    .Routing(configurer.ApplyActions);
            }
        }


        /// <summary>
        /// Configures services bus with additional action
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureServiceBus(this IServiceCollection services, Action<ICqrsConfigurer> configure)
        {
            configure?.Invoke(GetCqrsConfigurer(services));

            return services;
        }

        private static CqrsConfigurer GetCqrsConfigurer(IServiceCollection services)
        {
            ServiceDescriptor descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(CqrsConfigurer));
            if (descriptor == null)
            {
                var configurer = new CqrsConfigurer(services);
                services.AddSingleton(configurer);

                return configurer;
            }

            var cqrsConfigurer = (CqrsConfigurer) descriptor.ImplementationInstance;
            // IServiceCollection instance can change
            cqrsConfigurer.Services = services;

            return cqrsConfigurer;
        }

        private static JsonSerializerSettings GetJsonSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new MessageContractResolver()
            };
            return settings;
        }

        private class RebusHostedService : IHostedService
        {
            private readonly CqrsConfigurer _cqrsConfigurer;
            private readonly IServiceProvider _serviceProvider;

            public RebusHostedService(IServiceProvider serviceProvider, CqrsConfigurer cqrsConfigurer)
            {
                _serviceProvider = serviceProvider;
                _cqrsConfigurer = cqrsConfigurer;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                // Activate bus
                var bus = _serviceProvider.GetRequiredService<IBus>();

                await _cqrsConfigurer.ApplySubscriptionsAsync(bus);
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}