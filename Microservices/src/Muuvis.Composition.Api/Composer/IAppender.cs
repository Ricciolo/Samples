using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Muuvis.Composition.Api.Composer
{
    public interface IAppender
    {
        string Template { get; }

        Task<IAppenderResult> GetAsync(string url, CancellationToken token);
    }

}
