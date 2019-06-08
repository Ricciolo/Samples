using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.EntityFramework.DataModel;

namespace Muuvis.Catalog.EntityFramework
{
    internal class ContextHostedServices : IHostedService
    {
        private readonly ILogger<ContextHostedServices> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ContextHostedServices(ILogger<ContextHostedServices> logger, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var configurationContext = scope.ServiceProvider.GetRequiredService<CatalogContext>();

                if (configurationContext.Database.IsSqlServer())
                {
                    _logger.LogInformation("Ensuring database exists using connection string {connectionString}", configurationContext.Database.GetDbConnection().ConnectionString);

                    if (configurationContext.Database.GetMigrations().Any())
                    {
                        await configurationContext.Database.MigrateAsync(cancellationToken);
                    }
                    else
                    {
                        await configurationContext.Database.EnsureCreatedAsync(cancellationToken);
                    }

                    _logger.LogInformation("Database ready");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
