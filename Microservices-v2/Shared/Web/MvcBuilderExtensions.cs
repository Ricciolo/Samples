using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder SetODataFormatters(this IMvcBuilder builder)
        {
            builder.Services.Configure<MvcOptions>(o =>
            {
                foreach (var outputFormatter in o.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                }

                foreach (var outputFormatter in o.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                }
            });

            return builder;
        }
    }
}
