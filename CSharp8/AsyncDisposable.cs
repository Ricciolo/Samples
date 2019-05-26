using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class AsyncDisposable
    {
        async void Test()
        {
            await using var test = new Test();
        }
    }

    class Test : IAsyncDisposable
    {
        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
