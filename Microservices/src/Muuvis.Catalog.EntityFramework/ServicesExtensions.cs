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
using Muuvis.Catalog.DomainModel;
using Muuvis.Catalog.EntityFramework.DataAccessObject;
using Muuvis.Catalog.EntityFramework.DataModel;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddCatalogEntityFrameworkRepositories(this IServiceCollection services, Action<DbContextOptionsBuilder> options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddDbContext<CatalogEntities>(options, lifetime);
            services.AddSingleton<IHostedService, EntitiesHostedServices>();

            services.TryAddTransient<IRepository<Movie>, DbRepository<Movie, CatalogEntities>>();

            return services;
        }

        public static IServiceCollection AddCatalogEntityFrameworkDataAccessObjects(this IServiceCollection services)
        {
            services.TryAddTransient<IDataAccessObject<Muuvis.Catalog.ReadModel.Movie>, MovieDataAccessObject>();

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
                    var c = scope.ServiceProvider.GetRequiredService<CatalogEntities>().Database.GetDbConnection();
                    DatabaseFacade database = scope.ServiceProvider.GetRequiredService<CatalogEntities>().Database;
                    //await database.EnsureDeletedAsync(cancellationToken);
                    await database.EnsureCreatedAsync(cancellationToken);
                }

                //using (var scope = _serviceProvider.CreateScope())
                //{
                //    var movieRepository = scope.ServiceProvider.GetRequiredService<IRepository<Movie>>();
                //    var movie = new Movie("Test");
                //    movieRepository.AddAsync(movie).GetAwaiter().GetResult();

                //    var movie2 = await movieRepository.GetAsync(movie.Id);
                //}
                //using (var scope = _serviceProvider.CreateScope())
                //{
                //    var dataAccessObject = scope.ServiceProvider.GetRequiredService<IDataAccessObject<Muuvis.Catalog.ReadModel.Movie>>();
                //    Muuvis.Catalog.ReadModel.Movie[] movies = await dataAccessObject.ToArrayAsync(cancellationToken);
                //}
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
