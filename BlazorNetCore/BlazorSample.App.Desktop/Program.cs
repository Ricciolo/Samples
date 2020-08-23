using System;
using WebWindows.Blazor;

namespace BlazorSample.App.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            ComponentsDesktop.Run<Startup>("My Blazor App", "wwwroot/index.html");
        }
    }
}
