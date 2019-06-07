using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Industria4
{
    public class GenericException : Exception
    {
        public EventId EventId { get; }

        public GenericException(EventId eventId, string message, Exception innerException) : base(message, innerException)
        {
            EventId = eventId;
        }

        protected GenericException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
