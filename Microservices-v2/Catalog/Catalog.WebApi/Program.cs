using System;
using System.ComponentModel;
using System.Globalization;
using Muuvis.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Muuvis.Depedencies;

namespace Muuvis.Catalog.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .CheckDependencies(c => c.AddSqlConnectionsCheck().AddRabbitMQCheck())
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((c, b) =>
                {
                    // Support devspaces env
                    if (c.HostingEnvironment.IsDevelopment() && c.HostingEnvironment.IsKubernetes())
                    {
                        b.AddJsonFile("appsettings.DevSpaces.json", false);
                    }
                })
                .UseSerilogWithConfiguration()
                .UseStartup<Startup>();
        }

    }
}
