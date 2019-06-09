using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muuvis.Cqrs;
using Muuvis.Web.Cqrs.Filters;
using Newtonsoft.Json.Converters;
using NSwag.AspNetCore;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Muuvis.Taste.WebApi
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
            services.AddTasteEntityFramework(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddServiceBus(b =>
            {
                b.AddTasteCommandsRoute();
                b.AddTasteHandlers();
                if (!HostingEnvironment.IsUnix())
                {
                    b.UseTasteInMemoryQueue();
                }
                else if (Configuration.IsRabbitMqConfigured())
                {
                    b.UseTasteRabbitQueue();
                }
                else if (Configuration.IsAzureServiceBusConfigured())
                {
                    b.UseTasteAzureServiceBus();
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
