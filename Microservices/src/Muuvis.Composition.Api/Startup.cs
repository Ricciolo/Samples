using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Muuvis.Composition.Api.Composer;

namespace Muuvis.Composition.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddComposition();
            //services.AddMvcCore().AddJsonFormatters()
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouter(r =>
            {
                r.MapCompositionRule(c => c.Route("v1/movies/{id}")
                    .MatchBy("id")
                    .ForwardToSingleHttp("http://muuvis.catalog.api/movie/{id}")
                    .ForwardToSingleHttp("http://muuvis.taste.api/suggestion/{id}"));

                r.MapCompositionRule(c => c.Route("v1/movies")
                    .MatchBy("id")
                    .ForwardToListHttp("http://muuvis.catalog.api/movie")
                    .ForwardToListHttp("http://muuvis.taste.api/suggestion"));
            });
        }
    }
}
