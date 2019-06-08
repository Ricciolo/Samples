using Muuvis.Cqrs;
using Muuvis.Web;
using Muuvis.Web.Cqrs.Filters;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;

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
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .SetODataFormatters()
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.SwaggerDoc("v1", new Info { Title = GetType().Assembly.FullName, Version = "v1" });
            });

            services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (HostingEnvironment.IsDevelopment())
            {
                app.UseCors();
            }

            app.UseMvc(o =>
            {
                o.EnableCommonOData();
            });

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipes");
            //});
        }
    }

}
