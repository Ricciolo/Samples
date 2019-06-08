using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Muuvis.Http;
using Polly;
using Polly.Extensions.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddTypedHttpClient<T>(this IServiceCollection services, Func<HttpClient, Uri, T> factory)
            where T : class
        {
            return services.AddHttpClient<T>()
                .AddTypedClient((c, p) =>
                {
                    var serviceOptions = p.GetRequiredService<IOptions<ServiceOptions>>();
                    Uri uri = serviceOptions.Value.GetFor<T>();
                    return factory(c, uri);
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(200, retryAttempt)));
        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }
    }
}
