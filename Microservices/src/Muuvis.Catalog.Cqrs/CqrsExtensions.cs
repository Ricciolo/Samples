using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Muuvis.Catalog.Cqrs;
using Muuvis.Cqrs;
using Rebus.Routing;
using Rebus.Routing.TypeBased;


// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
    public static class CqrsExtensions
    {
        public static ICqrsConfigurer AddCatalogQueue(this ICqrsConfigurer configurer)
        {
            configurer.AddQueue(Constants.CommandsQueueName);

            return configurer;
        }

        public static ICqrsConfigurer AddCatalogCommands(this ICqrsConfigurer configurer)
        {
            configurer.AddCommandsFromAssemblyOfType<AddMovieCommand>(Constants.CommandsQueueName);

            return configurer;
        }
    }
}
