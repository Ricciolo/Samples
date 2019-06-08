using System.Linq;

namespace Muuvis.DataAccessObject
{
    /// <summary>
    ///     Represents an object for querying model
    /// </summary>
    public interface IDataAccessObject : IQueryable
    {
    }

    /// <summary>
    ///     Represents an object for querying model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataAccessObject<out T> : IDataAccessObject, IQueryable<T>
    {
    }
}