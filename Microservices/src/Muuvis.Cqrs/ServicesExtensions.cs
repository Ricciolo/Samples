using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Muuvis.Cqrs;
using Muuvis.Cqrs.Rebus;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services, Action<ICqrsConfigurer> configurerAction)
        {
            // Numero di richieste contemporaneamente gestite
            int w = Environment.ProcessorCount * 10;

            var configurer = new CqrsConfigurer(services);
            configurerAction(configurer);

            services.AddTransient<IncludePrincipalClaimsStep>();

            services.AddRebus((configure, provider) => configure
                .Serialization(s => s.UseNewtonsoftJson())
                .Options(o =>
                {
                    o.EnableSynchronousRequestReply(replyMaxAgeSeconds: 30);
                    o.SetNumberOfWorkers(w);
                    o.SetMaxParallelism(w);
                    o.IncludePrincipalClaims(provider);
                    o.SimpleRetryStrategy(maxDeliveryAttempts: 2, secondLevelRetriesEnabled: true);
                })
                .Logging(l => l.Serilog())
                .Transport(configurer.ApplyActions)
                .Routing(configurer.ApplyActions));

            services.AddSingleton<IHostedService, RebusHostedService>(r => new RebusHostedService(r, configurer));

            return services;
        }

        private class RebusHostedService : IHostedService
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly CqrsConfigurer _cqrsConfigurer;

            public RebusHostedService(IServiceProvider serviceProvider, CqrsConfigurer cqrsConfigurer)
            {
                _serviceProvider = serviceProvider;
                _cqrsConfigurer = cqrsConfigurer;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                // Attiva il bus
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
