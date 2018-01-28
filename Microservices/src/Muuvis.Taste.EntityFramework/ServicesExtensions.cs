using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Muuvis.DataAccessObject;
using Muuvis.EntityFramework.DataAccessObject;
using Muuvis.EntityFramework.Repository;
using Muuvis.Repository;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Muuvis.Taste.DomainModel;
using Muuvis.Taste.EntityFramework.DataAccessObject;
using Muuvis.Taste.EntityFramework.DataModel;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddTasteEntityFrameworkRepositories(this IServiceCollection services, Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddDbContext<TasteEntities>(options, lifetime);
            services.AddSingleton<IHostedService, EntitiesHostedServices>();

            services.TryAddTransient<IRepository<Suggestion>, DbRepository<Suggestion, TasteEntities>>();

            return services;
        }

        public static IServiceCollection AddTasteEntityFrameworkDataAccessObjects(this IServiceCollection services)
        {
            services.TryAddTransient<IDataAccessObject<Muuvis.Taste.ReadModel.Suggestion>, SuggestionDataAccessObject>();

            return services;
        }

        private class EntitiesHostedServices : IHostedService
        {
            private readonly IServiceProvider _serviceProvider;

            public EntitiesHostedServices(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var c = scope.ServiceProvider.GetRequiredService<TasteEntities>().Database.GetDbConnection();
                    DatabaseFacade database = scope.ServiceProvider.GetRequiredService<TasteEntities>().Database;
                    await database.EnsureCreatedAsync(cancellationToken);
                }
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
