using Industria4.Web;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query.Expressions;
using Microsoft.OData;
using Microsoft.OData.UriParser;
using ServiceLifetime = Microsoft.OData.ServiceLifetime;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing
{
    /// <summary>
    /// Extensions for <see cref="IRouteBuilder"/>
    /// </summary>
    public static class RouteBuilderExtensions
    {
        /// <summary>
        /// Enables common OData functionality like pagination, filtering, selection
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IRouteBuilder EnableCommonOData(this IRouteBuilder builder)
        {
            builder.EnableDependencyInjection(o =>
            {
                o.AddService(
                    ServiceLifetime.Singleton,
                    _ =>
                    {
                        var resolver = new ODataUriResolver { EnableCaseInsensitive = true };
                        return resolver;
                    });
            });
            builder.Filter().Count().Select().MaxTop(200).OrderBy();
            return builder;
        }
    }
}