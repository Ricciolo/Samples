using Muuvis.DomainModel;
using System;
using System.Threading.Tasks;

namespace Muuvis.Repository
{
    /// <summary>
    /// Base implementation of repository which takes care of casting object to a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        /// <summary>
        /// Adds the object to the repository 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract Task OnAddAsync(T entity);

        /// <summary>
        /// Updates the object contained into the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract Task OnUpdateAsync(T entity);

        /// <summary>
        /// Removes the object from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract Task OnRemoveAsync(string id);

        /// <summary>
        /// Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract Task<T> OnGetAsync(string id);


        /// <summary>
        /// Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract Task<bool> OnExistAsync(string id);

        #region IRepository<T>

        /// <summary>
        ///     Adds the object to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return OnAddAsync(entity);
        }

        /// <summary>
        ///     Updates the object contained into the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return OnUpdateAsync(entity);
        }

        /// <summary>
        ///     Removes the object from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task RemoveAsync(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            return OnRemoveAsync(id);
        }

        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> GetAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return OnGetAsync(id);
        }


        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> ExistAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return OnExistAsync(id);
        }

        #endregion

        #region IRepository

        /// <summary>
        ///     Adds the object to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task AddAsync(object entity)
        {
            return AddAsync(GetEntity(entity));
        }

        /// <summary>
        ///     Updates the object contained into the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task UpdateAsync(object entity)
        {
            return AddAsync(GetEntity(entity));
        }

        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> GetObjectAsync(string id)
        {
            return await GetAsync(id);
        }

        #endregion

        private T GetEntity(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (!(entity is T tentity))
                throw new ArgumentException($"Invalid entity type. Must be {typeof(T)}");

            return tentity;
        }
    }
}
