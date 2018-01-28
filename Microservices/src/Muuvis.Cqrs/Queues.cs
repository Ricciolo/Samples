using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Muuvis.Cqrs
{
    public static class Queues
    {
        public static readonly CatalogQueues Catalog = new CatalogQueues();

        public static readonly TasteQueues Taste = new TasteQueues();

        public class CatalogQueues
        {
            protected internal CatalogQueues()
            {
                
            }

            public readonly string CommandsQueueName = "Catalog-Commands";

            public readonly string EventsQueueName = "Catalog-Events";

        }
        public class TasteQueues
        {
            protected internal TasteQueues()
            {

            }

            public readonly string CommandsQueueName = "Taste-Commands";

            public readonly string EventsQueueName = "Taste-Events";

        }
    }
}
