using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Serilog.Formatting.Json;

// ReSharper disable once CheckNamespace
namespace Serilog
{
    public static class LoggerConfigurationExtensions
    {
        public static void ConfigureLogger(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            LoggerConfiguration lg = loggerConfiguration
                .ReadFrom.Configuration(configuration);                

            string logDir;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                logDir = $"/var/log/containers/{Assembly.GetEntryAssembly().GetName().Name}";
            }
            else
            {
                logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Process.GetCurrentProcess().ProcessName);
                lg.WriteTo.ColoredConsole();
            }

            lg.WriteTo.RollingFile(new JsonFormatter(), $"{logDir}/log-{{Date}}.log", shared: true, fileSizeLimitBytes: 1024 * 1024 * 500, retainedFileCountLimit: 7);
            if (Debugger.IsAttached)
            {
                lg.WriteTo.Debug();
            }
        }
    }
}
