using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Muuvis.Repository
{
    public class RepositoryException : GenericException
    {
        public RepositoryException(EventId eventId, string message, Exception innerException) : base(eventId, message, innerException)
        {
        }

        protected RepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

		public const int VIOLATION_UNIQUE_KEY_ERROR_NUMBER = 19;

	}
}
