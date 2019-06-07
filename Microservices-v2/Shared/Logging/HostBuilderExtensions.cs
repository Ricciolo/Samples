using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Core;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    ///     Extensions for <see cref="IHostBuilder" />
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        ///     Configures Serilog as logger, adds it to the service list and configures Serilog from configuration
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseSerilogWithConfiguration(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((h, collection) =>
            {
                var loggerConfiguration = new LoggerConfiguration();
                loggerConfiguration.ConfigureLogger(h.Configuration);

                Logger logger = loggerConfiguration.CreateLogger();
                Log.Logger = logger;

                collection.AddSingleton(services => (ILoggerFactory) new SerilogLoggerFactory(null, true));
                collection.AddSingleton(p => Log.Logger);
            });
        }
    }
}