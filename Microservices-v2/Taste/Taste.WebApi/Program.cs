using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Muuvis.Depedencies;
using Muuvis.Web;

namespace Muuvis.Taste.WebApi
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
