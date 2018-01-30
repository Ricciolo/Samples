using System.Collections.Generic;

namespace Muuvis.Composition.Api.Composer
{
    public interface IAppenderResult
    {

    }


    public interface IAppenderSingleResult : IAppenderResult
    {
        dynamic Result { get; }
    }

    public interface IAppenderListResult : IAppenderResult
    {
        IEnumerable<dynamic> Result { get; }
    }

    public class AppenderSingleResult : IAppenderSingleResult
    {
        public AppenderSingleResult(dynamic result)
        {
            Result = result;
        }

        public dynamic Result { get; }
    }

    public class AppenderListResult : IAppenderListResult
    {
        public AppenderListResult(IEnumerable<dynamic> result)
        {
            Result = result;
        }

        public IEnumerable<dynamic> Result { get; }
    }
}