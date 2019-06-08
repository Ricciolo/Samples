using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.WebApi;

// ReSharper disable once CheckNamespace
namespace NSwag.AspNetCore
{
    public static class SwaggerExtensions
    {
        public static void HandlePathBase(this SwaggerUi3Settings<WebApiToSwaggerGeneratorSettings> settings, IConfiguration configuration)
        {
            string basePath = configuration.GetValue<string>("basePath");
            if (!String.IsNullOrWhiteSpace(basePath))
            {
                settings.DocumentPath = $"{basePath}/swagger/{{documentName}}/swagger.json";
            }
        }
    }
}
