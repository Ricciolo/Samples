using System.Net.Http;
using Microsoft.Azure.Devices.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PhotoCollector
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;

    class Program
    {
        private static string Storage;

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Init(cts.Token);

            // Wait until the app unloads or is cancelled
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Initializes the ModuleClient and sets up the callback to receive
        /// messages containing temperature information
        /// </summary>
        static async void Init(CancellationToken token)
        {
            AmqpTransportSettings amqpSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
            ITransportSettings[] settings = { amqpSetting };

            // Apre la connessione
            ModuleClient ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await ioTHubModuleClient.OpenAsync(token);

            // Recupera la connessione allo storage
            Storage = Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING");
            Console.WriteLine($"Storage: {Storage}");

            await ioTHubModuleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyUpdate, null, token);

            Console.WriteLine("IoT Hub module client initialized.");

            while (!token.IsCancellationRequested)
            {
                await Task.Delay(5000, token);

                try
                {
                    await SendPhoto(ioTHubModuleClient, token);
                }
                catch (Exception ex) when (!(ex is OperationCanceledException))
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static Task OnDesiredPropertyUpdate(TwinCollection desiredproperties, object usercontext)
        {
            if (!desiredproperties.Contains("storage"))
            {
                Console.WriteLine("No storage property is available");
                return Task.CompletedTask;
            }
            Storage = desiredproperties["storage"];
            return Task.CompletedTask;
        }

        private static async Task SendPhoto(ModuleClient ioTHubModuleClient, CancellationToken token)
        {
            if (Storage == null) return;

            CloudStorageAccount account = CloudStorageAccount.Parse(Storage);
            Console.WriteLine($"Connecting to {account.BlobEndpoint}");
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            // Creazione del container
            Console.WriteLine("Preparing blob container");
            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference("photos");
            await cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, new BlobRequestOptions(), null, token);

            var client = new HttpClient();
            string photoUri;
            // Download dell'immagine
            Console.WriteLine("Loading photo");
            using (Stream source = await client.GetStreamAsync("https://picsum.photos/1024/768"))
            {
                CloudBlockBlob blobReference = cloudBlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + ".jpg");
                blobReference.Properties.ContentType = "image/jpeg";

                // Upload sullo storage
                Console.WriteLine("Uploading photo");
                await blobReference.UploadFromStreamAsync(source);

                photoUri = blobReference.Uri.ToString();
            }

            // Invio dell'URI
            byte[] data = Encoding.UTF8.GetBytes(photoUri);
            var message = new Message(data);
            await ioTHubModuleClient.SendEventAsync("photo", message, token);
        }
    }
}
