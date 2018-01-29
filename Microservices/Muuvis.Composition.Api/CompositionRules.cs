using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;

namespace Muuvis.Composition.Api
{
    public class CompositionRules
    {
        public void Add(string sourceTemplate, string routeUrlTemplate)
        {
            RouteTemplate sourceRouteTemplate = TemplateParser.Parse(sourceTemplate);
        }
    }
}
