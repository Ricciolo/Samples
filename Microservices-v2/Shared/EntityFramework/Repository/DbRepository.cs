using System;
using System.Threading.Tasks;
using Muuvis.DomainModel;
using Muuvis.Repository;
using Microsoft.EntityFrameworkCore;

namespace Muuvis.EntityFramework.Repository
{
    /// <summary>
    ///     Base repository implementation based on <see cref="DbContext" /> dedicated to a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public class DbRepository<T, TDbContext> : Repository<T>
        where T : class, IEntity
        where TDbContext : DbContext
    {
        public DbRepository(TDbContext context)
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

        protected override async Task OnAddAsync(T entity)
        {
            DbSet.Add(entity);

            await Context.SaveChangesAsync();
        }

        protected override async Task OnUpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;

            await Context.SaveChangesAsync();
        }

        protected override async Task OnRemoveAsync(string id)
        {
            T entity = await DbSet.FindAsync(id);

            Context.Entry(entity).State = EntityState.Deleted;
            await Context.SaveChangesAsync();
        }

        protected override Task<T> OnGetAsync(string id)
        {
            return Context.FindAsync<T>(id);
        }

        protected override async Task<bool> OnExistAsync(string id)
        {
            var dataModel = await Context.FindAsync<T>(id);
            return dataModel != null;
        }

    }
}