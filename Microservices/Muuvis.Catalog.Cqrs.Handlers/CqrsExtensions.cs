using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Muuvis.Catalog.Cqrs;
using Muuvis.Catalog.Cqrs.Handlers;
using Muuvis.Cqrs;
using Rebus.Routing;
using Rebus.Routing.TypeBased;


// ReSharper disable once CheckNamespace
namespace Muuvis.Cqrs
{
    public static class CqrsExtensions
    {
        public static ICqrsConfigurer AddCatalogCommandHandlers(this ICqrsConfigurer configurer)
        {
            configurer.AddCommandHandlersFromAssemblyOfType<MovieHandlers>();

            return configurer;
        }

    }
}
