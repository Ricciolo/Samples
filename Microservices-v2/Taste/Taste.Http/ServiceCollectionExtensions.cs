using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Muuvis.Taste.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddTasteHttpClient(this IServiceCollection services)
        {
            return services.AddTypedHttpClient((c, u) => new TasteClient(u.ToString(), c));
        }

     
    }
}
