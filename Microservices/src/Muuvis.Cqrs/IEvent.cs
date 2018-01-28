using System;
using System.Collections.Generic;
using System.Text;

namespace Muuvis.Cqrs
{
    public interface IEvent : IMessage
    {
    }

    public abstract class EventBase : IEvent
    {
    }
}
