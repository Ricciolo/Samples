using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Muuvis.Composition.Api
{
    public class CompositionMiddleware
    {
        private readonly RequestDelegate _next;

        public CompositionMiddleware(RequestDelegate next, CompositionRules compositionRules)
        {
            _next = next;
        }

        public async Task Next(HttpContext context)
        {
            await _next(context);
        }
    }
}
