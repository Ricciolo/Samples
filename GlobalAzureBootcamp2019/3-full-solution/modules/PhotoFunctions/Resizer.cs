using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.WebJobs.Extensions.EdgeHub;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PhotoFunctions
{
    public static class Resizer
    {
        private static string Storage;

        static Resizer()
        {
            // Recupera la connessione allo storage
            Storage = Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING");
        }

        [FunctionName("Resizer")]
        public static async Task Run(
            [EdgeHubTrigger("resizer")]Message message,
            [EdgeHub(OutputName = "imageInfo")]IAsyncCollector<Message> imageInfoMessages,
            ILogger log)
        {
            string uri = Encoding.UTF8.GetString(message.GetBytes());
            log.LogInformation("Received {uri}", uri);

            // Salva l'immagine ridimensionata nel blob
            (int width, int height, long size) = await ResizeImageAsync(log, uri);

            // Invia le informazioni sull'immagine
            string json = JsonConvert.SerializeObject(new { width, height, size });
            message = new Message(Encoding.UTF8.GetBytes(json));
            await imageInfoMessages.AddAsync(message);
        }


        private static async Task<(int width, int height, long size)> ResizeImageAsync(ILogger log, string uri)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(Storage);
            log.LogInformation("Connecting to {blobEndpoint}", account.BlobEndpoint);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            // Creazione del container
            log.LogInformation("Preparing blob container");
            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference("thumbnails");
            await cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, new BlobRequestOptions(), null);

            using (var sourceStream = new MemoryStream())
            using (var targetStream = new MemoryStream())
            {
                ICloudBlob sourceBlob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(uri));
                await sourceBlob.DownloadToStreamAsync(sourceStream);
                sourceStream.Position = 0;

                using (Image<Rgba32> sourceImage = Image.Load(sourceStream))
                {
                    var result = (width: sourceImage.Width, height: sourceImage.Height, size: sourceStream.Length);
                    sourceImage.Mutate(m => m.Resize(150, 150).Grayscale());
                    sourceImage.SaveAsJpeg(targetStream);

                    targetStream.Position = 0;
                    CloudBlockBlob targetBlob = cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(uri));
                    targetBlob.Properties.ContentType = "image/jpeg";
                    await targetBlob.UploadFromStreamAsync(targetStream);

                    log.LogInformation("Uploaded thumbnail to {blobEndpoint}", targetBlob.Uri);

                    return result;
                }
            }
        }
    }
}