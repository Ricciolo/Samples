using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

namespace Muuvis.Composition.Api.Composer
{
    public class CompositionRouteHandler
    {
        private readonly CompositionRule _rule;
        private readonly IAppender[] _appenders;
        private readonly Tuple<Uri, RouteTemplate>[] _routeTemplates;

        public CompositionRouteHandler(CompositionRule rule)
        {
            _rule = rule;

            _appenders = rule.Appenders.ToArray();
            _routeTemplates = rule.Appenders.Select(a => ParseTemplate(a.Template)).ToArray();
        }

        private Tuple<Uri, RouteTemplate> ParseTemplate(string template)
        {
            if (Uri.TryCreate(template, UriKind.Absolute, out Uri u))
            {
                return Tuple.Create(u, TemplateParser.Parse(u.LocalPath.Substring(1)));
            }

            return Tuple.Create((Uri)null, TemplateParser.Parse(template));
        }

        public async Task Execute(HttpContext context)
        {
            RouteData routeData = context.GetRouteData();

            var tasks = _appenders.Select((a, i) =>
            {
                // Create binder for the tempalte
                var templateBinder = new TemplateBinder(
                    context.RequestServices.GetRequiredService<UrlEncoder>(),
                    context.RequestServices.GetRequiredService<ObjectPool<UriBuildingContext>>(),
                    _routeTemplates[i].Item2,
                    null);

                // Transform the uri
                Uri fullUri = _routeTemplates[i].Item1 ?? context.Request.GetUri();
                string url = templateBinder.BindValues(new RouteValueDictionary(routeData.Values));

                var ub = new UriBuilder(fullUri);
                ub.Path = url;

                // Make the request
                return a.GetAsync(ub.ToString(), context.RequestAborted);
            });

            // Wait for answer from all requests
            IAppenderResult[] results = await Task.WhenAll(tasks);

            // Merge reults
            object result = MergeResults(results);

            // Write output using formatters
            var actionContext = new ActionContext {HttpContext = context, RouteData = routeData};
            var objectResult = new OkObjectResult(result);
            await objectResult.ExecuteResultAsync(actionContext);
        }

        private object MergeResults(IAppenderResult[] results)
        {
            IAppenderListResult[] appenderListResults = results.OfType<IAppenderListResult>().ToArray();
            IAppenderSingleResult[] appenderSingleResults = results.OfType<IAppenderSingleResult>().ToArray();

            if (appenderListResults.Length == results.Length)
            {
                return MergeListResults(appenderListResults);
            }
            else if (appenderSingleResults.Length == results.Length)
            {
                return MergeSingleResults(appenderSingleResults);
            }

            throw new NotSupportedException();
        }

        private dynamic MergeSingleResults(IAppenderSingleResult[] results)
        {
            return MergeObjects(results.Select(r => r.Result));
        }

        private dynamic MergeListResults(IAppenderListResult[] results)
        {
            return from r in results
                from l in r.Result
                let i = (IDictionary<string, object>) l
                group l by i.ContainsKey(_rule.MatchProperty) ? i[_rule.MatchProperty] : new object()
                into g
                select MergeObjects(g);
        }

        private dynamic MergeObjects(IEnumerable<dynamic> objects)
        {
            IDictionary<string, object> result = new ExpandoObject();

            foreach (IDictionary<string, object> dic in objects)
            {
                foreach (var pair in dic)
                {
                    result[pair.Key] = pair.Value;
                }
            }

            return result;
        }
    }
}
