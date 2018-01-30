using System;
using Microsoft.AspNetCore.Routing;

namespace Muuvis.Composition.Api.Composer
{
    public static class CompositionRouterExtensions
    {
        public static IRouteBuilder MapCompositionRule(this IRouteBuilder builder, CompositionRule rule)
        {
            var handler = new CompositionRouteHandler(rule);
            return builder.MapRoute(rule.RouteTemplate, handler.Execute);
        }

        public static IRouteBuilder MapCompositionRule(this IRouteBuilder builder, Action<CompositionRuleBuilder> compositionBuilder)
        {
            var b = new CompositionRuleBuilder(builder.ServiceProvider);
            compositionBuilder(b);
            return MapCompositionRule(builder, b.Build());
        }
    }
}
