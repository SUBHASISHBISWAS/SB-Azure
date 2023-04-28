using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Newtonsoft.Json;

namespace TodoAzureFunctionApp
{
    public class TodoQueueListener
    {
        [FunctionName("TodoQueueListener")]
        public async Task Run(
            [QueueTrigger("todos", Connection = "AzureWebJobsStorage")] Todo todoQueueItem,
            [Blob("todos",Connection = "AzureWebJobsStorage")] BlobContainerClient container,
            ILogger log)
        {
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlobClient($"{todoQueueItem.Id}.json");
            await blob.UploadTextAsync(JsonConvert.SerializeObject($"Created a new task: {todoQueueItem.TaskDescription}"));
            log.LogInformation($"C# Queue trigger function processed: {todoQueueItem.TaskDescription}");
        }
    }

    internal static class BlobClientExtensions
    {
        public static Task UploadTextAsync(this BlobClient client, string text)
        {
            var content = new BinaryData(text);
            return client.UploadAsync(content, true); // support overwrite as we use this to update blobs
        }

        public async static Task<string> DownloadTextAsync(this BlobClient client)
        {
            var res = await client.DownloadContentAsync();
            return res.Value.Content.ToString();
        }
    }
}
