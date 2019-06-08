using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Muuvis.Catalog.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddCatalogHttpClient(this IServiceCollection services)
        {
            return services.AddTypedHttpClient((c, u) => new CatalogClient(u.ToString(), c));
        }

     
    }
}
