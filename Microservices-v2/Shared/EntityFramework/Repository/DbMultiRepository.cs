using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Industria4.DomainModel;
using Industria4.Repository;
using Microsoft.EntityFrameworkCore;

namespace Industria4.EntityFramework.Repository
{
    /// <summary>
    ///     Base repository implementation based on <see cref="DbContext" /> dedicated to a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public class DbMultiRepository<T, TDbContext> : MultiRepository<T>
        where T : class, IEntity
        where TDbContext : DbContext
    {
        public DbMultiRepository(TDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        ///     Gets the current <see cref="DbContext" />
        /// </summary>
        public TDbContext Context { get; }

        /// <summary>
        ///     Gets the <see cref="DbSet" /> for the type
        /// </summary>
        protected virtual DbSet<T> DbSet => Context.Set<T>();

        protected override async Task OnAddAsync(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);

            await Context.SaveChangesAsync();
        }

        protected override async Task OnUpdateAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            await Context.SaveChangesAsync();
        }

        protected override async Task OnRemoveAsync(IEnumerable<string> ids)
        {
            IEnumerable<T> entities = await GetAsync(ids);
            foreach (T entity in entities)
            {
                Context.Entry(entity).State = EntityState.Deleted;
            }

            await Context.SaveChangesAsync();
        }

        protected override async Task<IEnumerable<T>> OnGetAsync(IEnumerable<string> ids)
        {
            if (ids.Count() == 1)
            {
                return new[] { await DbSet.FindAsync(ids.First()) };
            }
            else
            {
                string[] idsArray = ids as string[] ?? ids.ToArray();
                return await DbSet.Where(i => idsArray.Contains(i.Id)).ToArrayAsync();
            }
        }

        protected override async Task<IEnumerable<bool>> OnExistAsync(IEnumerable<string> ids)
        {
            if (ids.Count() == 1)
            {
                return new[] { await DbSet.FindAsync(ids.First()) != null };
            }
            else
            {
                string[] idsArray = ids as string[] ?? ids.ToArray();
                string[] found = await DbSet.Where(i => idsArray.Contains(i.Id)).Select(i => i.Id).ToArrayAsync();

                return idsArray.Select(i => found.Contains(i)).ToArray();
            }
        }

    }
}