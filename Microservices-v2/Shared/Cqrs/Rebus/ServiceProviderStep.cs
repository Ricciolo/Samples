using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Pipeline;
using Rebus.Transport;

namespace Industria4.Cqrs.Rebus
{
    public class ServiceProviderStep : IOutgoingStep, IIncomingStep
    {
        private readonly IServiceProvider _serviceProvider;

        private static readonly AsyncLocal<IServiceProvider> Local = new AsyncLocal<IServiceProvider>();

        /// <summary>
        /// Gets or sets the IServiceProvider to use for next pipeline operations
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get => Local.Value;
            set => Local.Value = value;
        }

        public ServiceProviderStep(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Process(IncomingStepContext context, Func<Task> next)
        {
            SetScope(context);

            return next();
        }

        public Task Process(OutgoingStepContext context, Func<Task> next)
        {
            SetScope(context);

            return next();
        }

        private void SetScope(StepContext context)
        {
            var transactionContext = context.Load<ITransactionContext>();
            transactionContext.GetOrAdd(nameof(IServiceProvider), () =>
            {
                // Look into async flow value
                IServiceProvider provider = ServiceProvider;
                if (provider == null)
                {
                    // Create explicitly the scope
                    IServiceScope scope = _serviceProvider.CreateScope();
                    provider = ServiceProvider = scope.ServiceProvider;
                    // Register to dispose it
                    transactionContext.OnDisposed(scope.Dispose);
                    transactionContext.OnDisposed(() => { if (ServiceProvider == scope.ServiceProvider) { ServiceProvider = null; } });
                }

                return provider;
            });
        }
    }
}