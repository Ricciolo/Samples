using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Extensions;
using Rebus.Handlers;
using Rebus.ServiceProvider;
using Rebus.Transport;

namespace Industria4.Cqrs.Rebus
{
    /// <summary>
    /// Implementation of <see cref="IContainerAdapter"/> that is backed by a ServiceProvider
    /// </summary>
    /// <seealso cref="IContainerAdapter" />
    public class NetCoreServiceProviderContainerAdapterEx : IContainerAdapter
    {
        readonly IServiceProvider _provider;

        IBus _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetCoreServiceProviderContainerAdapter"/> class.
        /// </summary>
        /// <param name="provider">The service provider used to yield handler instances.</param>
        public NetCoreServiceProviderContainerAdapterEx(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));

            var applicationLifetime = _provider.GetService<IApplicationLifetime>();
            applicationLifetime?.ApplicationStopping.Register(Dispose);
        }

        /// <summary>
        /// Resolves all handlers for the given <typeparamref name="TMessage"/> message type
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public Task<IEnumerable<IHandleMessages<TMessage>>> GetHandlers<TMessage>(TMessage message, ITransactionContext transactionContext)
        {
            IServiceProvider provider = transactionContext.GetProvider();
            List<IHandleMessages<TMessage>> resolvedHandlerInstances = GetMessageHandlersForMessage<TMessage>(provider);

            return Task.FromResult((IEnumerable<IHandleMessages<TMessage>>)resolvedHandlerInstances.ToArray());
        }

        /// <summary>
        /// Sets the bus instance associated with this <see cref="T:Rebus.Activation.IContainerAdapter" />.
        /// </summary>
        /// <param name="bus"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetBus(IBus bus)
        {
            if (_bus != null)
            {
                throw new InvalidOperationException("Cannot set the bus instance more than once on the container adapter.");
            }

            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        private List<IHandleMessages<TMessage>> GetMessageHandlersForMessage<TMessage>(IServiceProvider serviceProvider)
        {
            var handledMessageTypes = typeof(TMessage).GetBaseTypes()
                .Concat(new[] { typeof(TMessage) });

            return handledMessageTypes
                .SelectMany(t =>
                {
                    var implementedInterface = typeof(IHandleMessages<>).MakeGenericType(t);

                    return serviceProvider.GetServices(implementedInterface).Cast<IHandleMessages>();
                })
                .Cast<IHandleMessages<TMessage>>()
                .ToList();
        }

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Disposes of the bus.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bus?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes of the bus.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
