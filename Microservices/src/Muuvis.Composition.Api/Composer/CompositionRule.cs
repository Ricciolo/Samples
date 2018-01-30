using Microsoft.AspNetCore.Routing.Template;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Muuvis.Composition.Api.Composer
{
    public class CompositionRule
    {
        public CompositionRule(string routeTemplate)
        {
            RouteTemplate = routeTemplate;
        }

        public string RouteTemplate { get; }

        public IList<IAppender> Appenders { get; } = new List<IAppender>();

        public string MatchProperty { get; set; } = "id";
    }

        
}
