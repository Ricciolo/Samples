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
            CreateBuilder(args)
                .Build()
                .CheckDependencies(c => c.AddSqlConnectionsCheck().AddRabbitMQCheck())
                .Run();
        }

        public static IWebHostBuilder CreateBuilder(params string[] args)
        {
            IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseSerilogWithConfiguration()
                .UseStartup<Startup>();

            return webHostBuilder;
        }

    }
}
