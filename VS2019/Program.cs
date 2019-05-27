using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VS2019
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: intellicode 1
            string s = "Test";
            if (s.Length == 0)
            {
                
            }

            var list = new List<string>();
            // TODO: intellicode 2

            // TODO: intellicode 3 - training

            // TODO: Live Share
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)              
               .UseStartup<Startup>();
    }
}
