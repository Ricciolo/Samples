using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Demo
{
    class AsyncStream
    {
        public AsyncStream()
        {
            Sample2().GetAwaiter().GetResult();
        }

        public void Sample1()
        {
            foreach (var t in GetFiles(@"c:\temp", "*.txt"))
            {
                break;
            }
        }

        public async Task Sample2()
        {          
            await foreach (string txt in GetFilesAsync(@"c:\temp", "*.txt"))
            {
                Console.WriteLine(txt.Length);
                break;
            }
        }

        public async Task Sample3()
        {
            await using IAsyncEnumerator<string> enumerator = GetFilesAsync(@"c:\temp", "*.txt").GetAsyncEnumerator();

            while (await enumerator.MoveNextAsync())
            {
                Console.WriteLine(enumerator.Current);
            }
        }

        private async IAsyncEnumerable<string> GetFilesAsync(string dir, string searchPattern)
        {
            try
            {
                foreach (var path in Directory.EnumerateFiles(dir, searchPattern))
                {
                    using Stream stream = File.OpenRead(path);
                    using StreamReader reader = new StreamReader(stream);

                    string content = await reader.ReadToEndAsync();
                    yield return content;
                }
            }
            finally
            {
                await Task.Delay(500);
            }
        }

        private IEnumerable<string> GetFiles(string dir, string searchPattern)
        {
            try
            {
                foreach (var path in Directory.EnumerateFiles(dir, searchPattern))
                {
                    using Stream stream = File.OpenRead(path);
                    using StreamReader reader = new StreamReader(stream);

                    string content = reader.ReadToEnd();
                    yield return content;
                }
            }
            finally
            {
                // qualcosa
            }
        }
    }
}
