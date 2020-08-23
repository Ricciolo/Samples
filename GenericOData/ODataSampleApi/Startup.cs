using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using ODataSample;
using ODataSampleApi.Component;

namespace ODataSampleApi
{
    public class Startup
    {
        private IEdmModel _edmModel;

        public Startup()
        {
            _edmModel = GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();

            services.AddControllers(o =>
                {
                    o.Conventions.Add(new GenericControllerRouteConvention(_edmModel));
                    o.EnableEndpointRouting = false;
                })
                .ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(typeof(Student).Assembly)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.EnableDependencyInjection();
                routeBuilder.Select().Filter().OrderBy().Count().MaxTop(100);
                routeBuilder.MapODataServiceRoute("odata", "odata", _edmModel);
            });
        }

        private IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Student>("Students");
            odataBuilder.EntitySet<Teacher>("Teachers");

            return odataBuilder.GetEdmModel();
        }
    }

}
