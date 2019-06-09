using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Muuvis.DataAccessObject;
using Muuvis.Repository;
using Muuvis.Taste.EntityFramework;
using Muuvis.Taste.EntityFramework.DataAccessObject;
using Muuvis.Taste.EntityFramework.DataModel;
using Muuvis.Taste.EntityFramework.Repositories;
using Muuvis.Taste.ReadModel;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServicesExtensions
    {

        private static void AddCommon(IServiceCollection services, Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime)
        {
            services.AddDbContext<TasteContext>(options, lifetime);
            services.TryAddSingleton<IHostedService, ContextHostedServices>();
            services.TryAddSingleton<TasteMapper>();
        }

        public static IServiceCollection AddTasteEntityFramework(this IServiceCollection services,
                                                                                Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AddTasteRepositories(services, options, lifetime);
            AddTasteDataAccessObjects(services, options, lifetime);

            return services;
        }

        public static IServiceCollection AddTasteRepositories(this IServiceCollection services,
                                                                                            Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AddCommon(services, options, lifetime);

            services.TryAddTransient<IRepository<Muuvis.Taste.DomainModel.Suggestion>, SuggestionRepository>();

            return services;
        }

        public static IServiceCollection AddTasteDataAccessObjects(this IServiceCollection services,
                                                                                                    Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AddCommon(services, options, lifetime);

            services.TryAddTransient<IDataAccessObject<Muuvis.Taste.ReadModel.Suggestion>, TasteDataAccessObject<Muuvis.Taste.EntityFramework.DataModel.Suggestion, Muuvis.Taste.ReadModel.Suggestion>>();

            return services;
        }
    }
}
