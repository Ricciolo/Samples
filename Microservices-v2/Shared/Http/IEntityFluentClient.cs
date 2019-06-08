using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Muuvis.Http
{
    /// <summary>
    /// Typed version of fluent client
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityFluentClient<TEntity>
    {
        /// <summary>
        /// Gets reference to the fluent client
        /// </summary>
        FluentClient Client { get; }
    }
}
