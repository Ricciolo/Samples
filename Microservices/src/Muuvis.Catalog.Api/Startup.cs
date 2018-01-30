using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Muuvis.Catalog.DomainModel;
using Muuvis.Cqrs;
using Muuvis.Repository;
using Rebus.Routing.Exceptions;

namespace Muuvis.Catalog.Api
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddUser(c => c.GetService<IHttpContextAccessor>()?.HttpContext?.User);
            services.AddCatalogEntityFrameworkRepositories(o => o.UseSqlServer(Configuration.GetConnectionString("Muuvis")));
            services.AddCatalogEntityFrameworkDataAccessObjects();

            services.AddSingleton(MapperConfig.Get);

            services.AddServiceBus(r => r
                .AddCatalogQueue()
                .AddCatalogCommandsRoute()
                .AddCatalogEventsRoute()
                .AddCatalogHandlers()
                .CatalogSubscribe());

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
