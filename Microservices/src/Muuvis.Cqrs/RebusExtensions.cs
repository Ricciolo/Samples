using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Muuvis.Common;
using Muuvis.Cqrs.Rebus;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;
using Rebus.Pipeline.Send;

namespace Muuvis.Cqrs
{
    public static class Extensions
    {
        //public static Task<IQueuedHandle> SendAsync(this IOperationSender sender, IOperationRequest request)
        //{
        //    return sender.SendAsync(request, null, null);
        //}

        public static void IncludePrincipalClaims(this OptionsConfigurer configurer, IServiceProvider provider)
        {
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();                
                var step = provider.GetRequiredService<IncludePrincipalClaimsStep>();

                return new PipelineStepInjector(pipeline)
                    .OnSend(step, PipelineRelativePosition.Before, typeof(AutoHeadersOutgoingStep));
            });
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();
                var step = provider.GetRequiredService<IncludePrincipalClaimsStep>();

                return new PipelineStepInjector(pipeline)
                    .OnReceive(step, PipelineRelativePosition.Before, typeof(DispatchIncomingMessageStep));
            });
        }

    }
}
