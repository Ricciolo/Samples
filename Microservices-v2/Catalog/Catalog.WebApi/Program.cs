using System.ComponentModel;
using System.Globalization;
using Muuvis.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
                .UseSerilogWithConfiguration()
                .UseStartup<Startup>();
        }

    }
}
