
using System;
using System.IO;
using FunctionApp.Models;
using FunctionApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace FunctionApp
{
    public static class Function1
    {
        static IServiceProvider ServiceProvider = Host.Create(s => {
            // Registrazioni per la funzione
            s.AddTransient<MyRepository>();
        });

        [FunctionName("AddItem")]
        public static IActionResult AddItem([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Called AddItem");

            // Deserializzo l'oggetto
            string json;
            using (var streamReader = new StreamReader(req.Body))
            {
                json = streamReader.ReadToEnd();
            }
            MyItem item = JsonConvert.DeserializeObject<MyItem>(json);

            // Chiamo il repository
            var repository = ServiceProvider.GetRequiredService<MyRepository>();
            repository.AddItem(item);

            return new OkResult();
        }
    }
}
