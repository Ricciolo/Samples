using System;
using System.Collections.Generic;
using System.Text;

namespace Muuvis.Cqrs.Events
{
    public class CompletionTimeoutEvent<T> : EventBase
    {
        public T Data { get; }

        public CompletionTimeoutEvent(T data)
        {
            Data = data;
        }
    }
}
