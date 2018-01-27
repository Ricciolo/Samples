using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Muuvis.DataAccessObject
{
    public interface IDataAccessObject : IQueryable
    {
    }

    public interface IDataAccessObject<out T> : IDataAccessObject, IQueryable<T>
    {

    }
}
