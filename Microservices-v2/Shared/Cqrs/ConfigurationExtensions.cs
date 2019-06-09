using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static bool IsRabbitMqConfigured(this IConfiguration configuration)
        {
            return !String.IsNullOrWhiteSpace(configuration["Cqrs:RabbitMqConnectionString"]);
        }

        public static bool IsAzureServiceBusConfigured(this IConfiguration configuration)
        {
            return !String.IsNullOrWhiteSpace(configuration["Cqrs:AzureServiceBusConnectionString"]);
        }
    }
}
