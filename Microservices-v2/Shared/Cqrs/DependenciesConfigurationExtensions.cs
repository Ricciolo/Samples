using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Industria4.Depedencies;

// ReSharper disable once CheckNamespace
namespace Industria4.Web
{
    public static class DependenciesConfigurationExtensions
    {
        public static DependenciesConfiguration AddRabbitMQCheck(this DependenciesConfiguration configuration)
        {
            IConfiguration config = configuration.Services.GetRequiredService<IConfiguration>();
            foreach (var pair in config.AsEnumerable()
                .Where(e => e.Key.IndexOf("RabbitMQ",StringComparison.OrdinalIgnoreCase) >= 0))
            {
                if (Uri.TryCreate(pair.Value, UriKind.Absolute, out Uri result))
                {
                    configuration.AddTcpCheck(result.Host, result.Port);
                }
            }

            return configuration;
        }
    }
}