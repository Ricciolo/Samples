using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WebWindows.Blazor;

namespace BlazorSample.App.Desktop
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44337/") });

            BlazorSample.App.Client.Program.ConfigureServices(services);
        }

        public void Configure(DesktopApplicationBuilder app)
        {
            app.AddComponent<Client.App>("app");
        }
    }
}
