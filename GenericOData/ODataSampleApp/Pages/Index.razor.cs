using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ODataSample;
using ODataSampleClient;
using Simple.OData.Client;

namespace ODataSampleApp.Pages
{
    public partial class Index
    {

        private string title;
        private IEnumerable<Student> students;
        private long count;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            title = "";
        }

        [Inject]
        public GenericClient<Student> OData { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var annotations = new ODataFeedAnnotations();

            students = await OData.Client
                //.Filter(s => s.Name.Contains("t"))
                .OrderBy(s => s.Id)
                .Top(10)
                .Select(s => new { s.Id, s.Name })
                .FindEntriesAsync(annotations);

            count = annotations.Count.GetValueOrDefault(0);
        }

        private async Task Insert()
        {
            var result = await OData.Client
                .Key(Guid.NewGuid())
                //.Set(new Student { Name = "test" })
                .Set(new { Name = "test" })
                .InsertEntryAsync(true);
                //.UpdateEntryAsync();

            title = result.Name;
        }
    }
}
