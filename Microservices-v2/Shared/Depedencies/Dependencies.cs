using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Muuvis.Depedencies
{
    public static class DepdendenciesExtensions
    {
        /// <summary>
        /// Adds a TCP connection check
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static DependenciesConfiguration AddTcpCheck(this DependenciesConfiguration configuration, string host, int port)
        {
            var c = new TcpCheck(configuration.Services.GetRequiredService<ILogger<TcpCheck>>(), host, port);
            return configuration.Add(c);
        }

        /// <summary>
        /// Adds a SQL Server check
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DependenciesConfiguration AddSqlConnectionCheck(this DependenciesConfiguration configuration, string connectionString)
        {
            var c = new SqlConnectionCheck(configuration.Services.GetRequiredService<ILogger<SqlConnectionCheck>>(), connectionString);
            return configuration.Add(c);
        }

        /// <summary>
        /// Adds a SQL Server check for all available connection strings
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DependenciesConfiguration AddSqlConnectionsCheck(this DependenciesConfiguration configuration)
        {
            IConfiguration config = configuration.Services.GetRequiredService<IConfiguration>();
            foreach (string value in config.GetSection("ConnectionStrings").GetChildren().AsEnumerable().Select(o => o.Value).Distinct())
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    configuration.AddSqlConnectionCheck(value);
                }
            }

            return configuration;
        }
    }
}
