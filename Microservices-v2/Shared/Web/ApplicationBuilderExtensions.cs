using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UsePathBase(this IApplicationBuilder builder)
        {
            var configuration = builder.ApplicationServices.GetRequiredService<IConfiguration>();
            string basePath = configuration.GetValue<string>("basePath");

            if (!string.IsNullOrEmpty(basePath))
            {
                builder.UsePathBase(basePath);
            }
        }
    }
}
