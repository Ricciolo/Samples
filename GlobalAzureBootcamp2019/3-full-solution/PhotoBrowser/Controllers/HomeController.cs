using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PhotoBrowser.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public async Task<IActionResult> Index()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING"));
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("thumbnails");

            BlobResultSegment segment = await container.ListBlobsSegmentedAsync(null);

            string[] images = segment.Results.Select(r => ChangeHost(r.Uri)).ToArray();

            return View(images);
        }

        private string ChangeHost(Uri uri)
        {
            var ub = new UriBuilder(uri);
            ub.Host = Request.Host.Host;
            return ub.ToString();
        }
    }
}