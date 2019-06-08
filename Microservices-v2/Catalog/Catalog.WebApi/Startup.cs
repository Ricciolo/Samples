using System.Linq;
using Muuvis.Cqrs;
using Muuvis.Web.Cqrs.Filters;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using NSwag;
using NSwag.AspNetCore;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Muuvis.Catalog.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCatalogEntityFramework(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddServiceBus(b =>
            {
                b.AddCatalogCommandsRoute();
                b.AddCatalogHandlers();
                if (!HostingEnvironment.IsUnix())
                {
                    b.UseCatalogInMemoryQueue();
                }
                else
                {
                    b.UseCatalogRabbitQueue();
                }
            });

            services.Configure<CqrsOptions>(Configuration.GetSection("Cqrs"));

            services.AddSingleton<WebApiMapper>();

            services.AddOData();

            services.AddHttpContextAccessor();
            services.AddMvc(o => o.Filters.Add(typeof(WaitCommandEventsAttribute)))
                .SetODataFormatters()
                .AddJsonOptions(o => { o.SerializerSettings.Converters.Add(new StringEnumConverter()); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddOpenApiDocument(s =>
            {
                s.Title = GetType().Assembly.GetName().Name;
                s.Version = "1.0";
            });

            services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UsePathBase();

            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (HostingEnvironment.IsDevelopment())
            {
                app.UseCors();
            }

            app.UseSwagger();
            app.UseSwaggerUi3(s => s.HandlePathBase(Configuration));

            app.UseMvc(o =>
            {
                o.EnableCommonOData();
            });
        }
    }

}
