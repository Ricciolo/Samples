using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muuvis.Cqrs;

namespace Muuvis.Taste.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUser(c => c.GetService<IHttpContextAccessor>()?.HttpContext?.User);
            services.AddTasteEntityFrameworkRepositories(o => o.UseSqlServer(Configuration.GetConnectionString("Muuvis")));
            services.AddTasteEntityFrameworkDataAccessObjects();

            services.AddServiceBus(r => r
                .AddTasteQueue()
                .AddTasteCommandsRoute()
                .AddTasteEventsRoute()
                .AddTasteHandlers()
                .TasteSubscribe());

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
