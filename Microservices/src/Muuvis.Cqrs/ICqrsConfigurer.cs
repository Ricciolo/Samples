using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Routing;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport;
using Rebus.Transport.InMem;

namespace Muuvis.Cqrs
{
    public interface ICqrsConfigurer
    {
        ICqrsConfigurer AddCommandsFromAssemblyOfType<T>(string queueName)
            where T : ICommand;

        ICqrsConfigurer AddQueue(string queueName);

        ICqrsConfigurer AddCommandHandlersFromAssemblyOfType<T>()
            where T : IHandleMessages;
    }

    internal class CqrsConfigurer : ICqrsConfigurer
    {
        private readonly IServiceCollection _services;
        private readonly List<Action<StandardConfigurer<IRouter>>> _routerActions = new List<Action<StandardConfigurer<IRouter>>>();
        private readonly List<Action<StandardConfigurer<ITransport>>> _transportActions = new List<Action<StandardConfigurer<ITransport>>>();

        public CqrsConfigurer(IServiceCollection services)
        {
            _services = services;
        }

        public ICqrsConfigurer AddCommandsFromAssemblyOfType<T>(string queueName)
            where T :ICommand
        {
            _routerActions.Add(r => r.TypeBased().MapAssemblyOf<T>(queueName));            

            return this;
        }

        public ICqrsConfigurer AddQueue(string queueName)
        {
            _transportActions.Add(t => t.UseInMemoryTransport(new InMemNetwork(), queueName));

            return this;
        }

        public ICqrsConfigurer AddCommandHandlersFromAssemblyOfType<T>() where T : IHandleMessages
        {
            _services.AutoRegisterHandlersFromAssemblyOf<T>();

            return this;
        }

        public void ApplyActions(StandardConfigurer<IRouter> routerConfigurer)
        {
            foreach (Action<StandardConfigurer<IRouter>> routerAction in _routerActions)
            {
                routerAction(routerConfigurer);
            }
        }

        public void ApplyActions(StandardConfigurer<ITransport> transportConfigurer)
        {
            foreach (Action<StandardConfigurer<ITransport>> transportAction in _transportActions)
            {
                transportAction(transportConfigurer);
            }
        }
    }
}
