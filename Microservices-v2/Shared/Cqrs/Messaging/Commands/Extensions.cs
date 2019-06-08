using System;
using System.Collections.Generic;
using System.Text;
using Rebus.Pipeline;
using Rebus.Transport;

namespace Muuvis.Cqrs.Messaging.Commands
{
    /// <summary>
    /// Extensions dedicated to the commands
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Disables the raising of an CommandCompletedEvent for the specified command in current context
        /// </summary>
        /// <param name="command"></param>
        public static void DisableAutoCompleteEvent(this ICommand command)
        {
            MessageContext.Current.TransactionContext.GetOrAdd(nameof(DisableAutoCompleteEvent), () => true);
        }

        /// <summary>
        /// Gets if the raising of an CommandCompletedEvent for the specified command in current context is disabled
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool GetIsAutoCompleteEventDisabled(this ITransactionContext context)
        {
            if (context.Items.TryGetValue(nameof(DisableAutoCompleteEvent), out object o))
                return (bool) o;
            return false;
        }
    }
}
