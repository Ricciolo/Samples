using System;
using Microsoft.Extensions.DependencyInjection;

namespace Muuvis.Composition.Api.Composer
{
    public class CompositionRuleBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private CompositionRule _rule =new CompositionRule("");

        public CompositionRuleBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CompositionRule Build()
        {
            return _rule;
        }

        public CompositionRuleBuilder MatchBy(string property)
        {
            _rule.MatchProperty = property;
            return this;
        }

        public CompositionRuleBuilder Route(string template)
        {
            CompositionRule old = _rule;
            _rule = new CompositionRule(template);
            if (old != null)
            {
                foreach (IAppender a in old.Appenders)
                {
                    _rule.Appenders.Add(a);
                }
            }
            return this;
        }

        public CompositionRuleBuilder ForwardToSingleHttp(string template)
        {
            var appender = _serviceProvider.GetRequiredService<HttpSingleAppender>();
            appender.Template = template;
            _rule.Appenders.Add(appender);

            return this;
        }

        public CompositionRuleBuilder ForwardToListHttp(string template)
        {
            var appender = _serviceProvider.GetRequiredService<HttpListAppender>();
            appender.Template = template;
            _rule.Appenders.Add(appender);

            return this;
        }
    }
}