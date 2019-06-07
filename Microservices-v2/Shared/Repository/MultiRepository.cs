using Industria4.DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Industria4.Repository
{
    /// <summary>
    /// Base implementation of repository which takes care of casting object to a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MultiRepository<T> : IRepository<T>, IMultiRepository<T>
        where T : class, IEntity
    {
        /// <summary>
        /// Adds the objects to the repository 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected abstract Task OnAddAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates the object contained into the repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected abstract Task OnUpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Removes the objects from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected abstract Task OnRemoveAsync(IEnumerable<string> ids);

        /// <summary>
        /// Gets the objects by id from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected abstract Task<IEnumerable<T>> OnGetAsync(IEnumerable<string> ids);


        /// <summary>
        /// Gets the objects by id from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected abstract Task<IEnumerable<bool>> OnExistAsync(IEnumerable<string> ids);

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
            return OnAddAsync(new[] { entity });
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
            return OnUpdateAsync(new[] { entity });
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
            return OnRemoveAsync(new[] { id });
        }

        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return (await OnGetAsync(new[] { id })).FirstOrDefault();
        }


        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return (await OnExistAsync(new[] { id })).FirstOrDefault();
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

        #region IMultiRepository<T>

        /// <summary>
        ///     Adds the objects to the repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task AddAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            return OnAddAsync(entities);
        }

        /// <summary>
        ///     Updates the objects contained into the repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task UpdateAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            return OnUpdateAsync(entities);
        }

        /// <summary>
        ///     Removes the objects from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task RemoveAsync(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            return OnRemoveAsync(ids);
        }

        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAsync(IEnumerable<string> ids)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));
            return OnGetAsync(ids);
        }


        /// <summary>
        ///     Gets the objects by id from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<IEnumerable<bool>> ExistAsync(IEnumerable<string> ids)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));
            return OnExistAsync(ids);
        }

        #endregion

        #region IMultiRepository

        /// <summary>
        ///     Adds the objects to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task AddAsync(IEnumerable<object> entities)
        {
            return AddAsync(GetEntities(entities));
        }

        /// <summary>
        ///     Updates the objects contained into the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task UpdateAsync(IEnumerable<object> entities)
        {
            return AddAsync(GetEntities(entities));
        }

        /// <summary>
        ///     Gets the objects by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetObjectAsync(IEnumerable<string> ids)
        {
            return await GetAsync(ids);
        }

        #endregion

        private IEnumerable<T> GetEntities(IEnumerable<object> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            return entities.Cast<T>();
        }

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
