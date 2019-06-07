using Industria4.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Industria4.Repository
{
    /// <summary>
    ///     Represents the repository pattern for generic objects
    /// </summary>
    public interface IMultiRepository
    {
        /// <summary>
        ///     Adds the objects to the repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddAsync(IEnumerable<object> entities);

        /// <summary>
        ///     Updates the objects contained into the repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<object> entities);

        /// <summary>
        ///     Removes the objects from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task RemoveAsync(IEnumerable<string> ids);

        /// <summary>
        ///     Gets the objects by id from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetObjectAsync(IEnumerable<string> ids);
    }

    /// <summary>
    ///     Represents the repository pattern for a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMultiRepository<T> : IMultiRepository
        where T : IEntity
    {
        /// <summary>
        ///     Adds the objects to the repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddAsync(IEnumerable<T> entities);

        /// <summary>
        ///     Updates the objects contained into the repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        ///     Gets the objects by id from the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync(IEnumerable<string> ids);

        /// <summary>
        ///     Check if objects exist by id on the repository
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<bool>> ExistAsync(IEnumerable<string> ids);

    }
}