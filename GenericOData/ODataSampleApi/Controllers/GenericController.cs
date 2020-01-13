using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODataSample;

namespace ODataSampleApi.Controllers
{
    public class GenericController<T> : ODataController
        where T : class
    {

        [HttpGet]
        [EnableQuery]
        public IEnumerable<T> Get()
        {
            return new Faker<T>().Generate(100);
        }

        [HttpPost]
        public IActionResult Post([FromBody]T T)
        {
            return Created(T);
        }

        [HttpPatch]
        public IActionResult Patch(Guid key, [FromBody]Delta<T> item)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(Guid key, [FromBody]T item)
        {
            return Updated(item);
        }

        [HttpGet]
        public ActionResult<T> Get(Guid key)
        {
            return new Faker<T>().Generate();
        }

        [HttpDelete]
        public IActionResult Delete(Guid key)
        {
            return Ok();
        }

        [HttpGet("/api/test")]
        public object Test()
        {
            return "ciai";
        }

    }
}
