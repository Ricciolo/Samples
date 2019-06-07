using Industria4.DomainModel;
using System.Threading.Tasks;

namespace Industria4.Repository
{
    /// <summary>
    ///     Represents the repository pattern for generic objects
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        ///     Adds the object to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(object entity);

        /// <summary>
        ///     Updates the object contained into the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(object entity);

        /// <summary>
        ///     Removes the object from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync(string id);

        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<object> GetObjectAsync(string id);
    }

    /// <summary>
    ///     Represents the repository pattern for a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IRepository
        where T : IEntity
    {
        /// <summary>
        ///     Adds the object to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(T entity);

        /// <summary>
        ///     Updates the object contained into the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity);

        /// <summary>
        ///     Gets the object by id from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAsync(string id);


        /// <summary>
        ///     Check if object exist by id on the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistAsync(string id);

    }
}