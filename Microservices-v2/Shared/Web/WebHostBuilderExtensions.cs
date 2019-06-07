using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    ///     Extensions for <see cref="IWebHostBuilder" />
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        ///     Configures Serilog as logger, adds it to the service list and configures Serilog from configuration
        /// </summary>
        /// <param name="webHostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseSerilogWithConfiguration(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureServices(c => c.AddSingleton(p => Log.Logger))
                .UseSerilog((b, c) => c.ConfigureLogger(b.Configuration));
        }

    }
}