using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Industria4.DataAccessObject;
using Microsoft.EntityFrameworkCore;

namespace Industria4.EntityFramework.DataAccessObject
{
    /// <summary>
    ///     Base object for querying model using <see cref="DbContext" />
    /// </summary>
    /// <typeparam name="TReadModel"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class DataAccessObject<TReadModel, TDbContext> : IDataAccessObject<TReadModel>, IAsyncEnumerable<TReadModel>
        where TReadModel : class
        where TDbContext : DbContext
    {
        protected DataAccessObject(TDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        ///     Gets the current <see cref="DbContext" />
        /// </summary>
        public TDbContext Context { get; set; }

        /// <summary>
        ///     Gets a <see cref="IQueryable" /> instance for current model
        /// </summary>
        protected abstract IQueryable<TReadModel> Query { get; }

        IAsyncEnumerator<TReadModel> IAsyncEnumerable<TReadModel>.GetEnumerator()
        {
            return ((IAsyncEnumerable<TReadModel>) Query).GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<TReadModel> GetEnumerator()
        {
            return Query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        Type IQueryable.ElementType => Query.ElementType;

        Expression IQueryable.Expression => Query.Expression;

        IQueryProvider IQueryable.Provider => Query.Provider;
    }
}