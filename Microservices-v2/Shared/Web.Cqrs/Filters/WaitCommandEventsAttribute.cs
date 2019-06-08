using Muuvis.Cqrs.Messaging;
using Muuvis.Cqrs.Messaging.Commands;
using Muuvis.Cqrs.Messaging.Events;
using Muuvis.Cqrs.Rebus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muuvis.Web.Cqrs.Filters
{

    /// <summary>
    /// Block any action for completetion until an event is received of any command involved into the action
    /// </summary>
    public class WaitCommandEventsAttribute : ActionFilterAttribute
    {
        public Type[] EventTypes { get; set; } = MessageTypesToWait;

        public bool Disabled { get; set; } = false;

        private static readonly Type[] MessageTypesToWait =
        {
            typeof(CommandCompletedEvent<>),
            typeof(CommandErrorEvent<>),
            typeof(CommandValidationEvent<>)
        };

        protected bool GetShouldHandle(ActionExecutingContext context)
        {
            // Verify that this filter is the latest one for the same time
            if (!context.Filters.OfType<WaitCommandEventsAttribute>().Last().Equals(this)) return false;

            HttpRequest request = context.HttpContext.Request;
            bool isDoAction = request.Method == HttpMethods.Post
                   || request.Method == HttpMethods.Put
                   || request.Method == HttpMethods.Delete;
            bool isAsync = request.Query.ContainsKey("async") || request.Headers.ContainsKey("x-async");

            return !Disabled && isDoAction && !isAsync;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!GetShouldHandle(context))
            {
                await next();
                return;
            }

            // Set the scope for current async flow
            ServiceProviderStep.ServiceProvider = context.HttpContext.RequestServices;

            var messagesCatcher = context.HttpContext.RequestServices.GetRequiredService<IMessagesCatcher>();
            var messagesWaiter = context.HttpContext.RequestServices.GetRequiredService<IMessagesWaiter>();
            var bus = context.HttpContext.RequestServices.GetRequiredService<IBus>();

            var tasks = new List<Task<IMessage>>();
            // Subscribe to event and create tasks for the events
            messagesCatcher.OnNewMessage = m => tasks.Add(SubscribeToEvents(bus, m, messagesWaiter));

            ActionExecutedContext resultContext = await next();
            
            // Wait for any command event for each group
            if (tasks.Count > 0)
            {
                try
                {
                    IMessage[] messages = await Task.WhenAll(tasks);
                    foreach (IMessage message in messages)
                    {
                        ProcessMessage(resultContext, message);
                    }
                }
                catch (TimeoutException)
                {
                    resultContext.Result = new StatusCodeResult(504);
                }
            }
        }

        private void ProcessMessage(ActionExecutedContext context, IMessage message)
        {
            switch (message)
            {
                case ICommandErrorEvent ce:
                    throw new AggregateException($"Error while execuding command {ce.CommandId} of type {ce.CommandType}", ce.Exceptions);
                case ICommandValidationEvent ce:
                    context.Result = new BadRequestObjectResult(new
                    {
                        ce.CommandType,
                        ce.CommandId,
                        ce.Results
                    });
                    break;
            }
        }

        private async Task<IMessage> SubscribeToEvents(IBus bus, IMessage message, IMessagesWaiter messagesWaiter)
        {
            // Handle only commands
            if (message is ICommand)
            {
                // Create events to subscribe for this command
                Type[] eventTypes = EventTypes.Select(t => t.IsGenericTypeDefinition ? t.MakeGenericType(message.GetType()) : t).ToArray();
                // Create wait task
                using (IMessageWaiter messageWaiter = await messagesWaiter.GetAsync(eventTypes, m => m is ICommandEvent ce && ce.CommandId == message.MessageId))
                {
                    return await messageWaiter.WhenAsync(TimeSpan.FromSeconds(30));
                }
            }

            return null;
        }


    }
}
