using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Muuvis.Catalog.EntityFramework;
using Muuvis.Catalog.EntityFramework.DataAccessObject;
using Muuvis.Catalog.EntityFramework.DataModel;
using Muuvis.Catalog.EntityFramework.Repositories;
using Muuvis.Catalog.ReadModel;
using Muuvis.DataAccessObject;
using Muuvis.Repository;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static partial class ServicesExtensions
	{

        private static void AddCommon(IServiceCollection services, Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime)
        {
            services.AddDbContext<CatalogContext>(options, lifetime);
            services.TryAddSingleton<IHostedService, ContextHostedServices>();
            services.TryAddSingleton<CatalogMapper>();
        }

        public static IServiceCollection AddCatalogEntityFramework(this IServiceCollection services,
                                                                                Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AddCatalogRepositories(services, options, lifetime);
            AddCatalogDataAccessObjects(services, options, lifetime);

            return services;
        }

        public static IServiceCollection AddCatalogRepositories(this IServiceCollection services,
                                                                                            Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AddCommon(services, options, lifetime);

            services.TryAddTransient<IRepository<Muuvis.Catalog.DomainModel.Movie>, MovieRepository>();

            return services;
        }

        public static IServiceCollection AddCatalogDataAccessObjects(this IServiceCollection services,
                                                                                                    Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AddCommon(services, options, lifetime);

            services.TryAddTransient<IDataAccessObject<MovieRead>, MovieDataAccessObject>();

            return services;
        }
    }
}
