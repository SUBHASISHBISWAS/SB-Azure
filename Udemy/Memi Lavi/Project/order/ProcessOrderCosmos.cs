using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.Storage.Blob;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using System.Text.Json;

namespace AzureCourse.Function
{
    public static class CosmosOrderFunction
    {
        [FunctionName("ProcessOrderCosmos")]
        public static void Run(
            [EventGridTrigger] EventGridEvent eventGridEvent,
            [Blob("neworders", FileAccess.Write, Connection = "StorageConnectionString")] CloudBlobContainer container,
            [CosmosDB(databaseName: "readit-orders", collectionName: "Orders", 
                ConnectionStringSetting = "CosmosDBConnection")] out Order order,
            ILogger log)
        {
            order = null;

            try
            {
                log.LogInformation("Function started");
                log.LogInformation($"Event details: Topic: {eventGridEvent.Topic}");
                log.LogInformation($"Event data: {eventGridEvent.Data.ToString()}");

                string eventBody = eventGridEvent.Data.ToString();

                log.LogInformation("Deserializing to StorageBlobCreatedEventData...");
                var storageData = JsonSerializer.Deserialize<StorageBlobCreatedEventData>(eventBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                log.LogInformation("Done");

                log.LogInformation("Get the name of the new blob...");
                var blobName = Path.GetFileName(storageData.Url);
                log.LogInformation($"Name of file: {blobName}");

                log.LogInformation("Get blob from storage...");
                var blockBlob = container.GetBlockBlobReference(blobName);
                var orderText = blockBlob.DownloadText();
                log.LogInformation("Done");
                log.LogInformation($"Order text: {orderText}");

                order = JsonSerializer.Deserialize<Order>(orderText, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error in function");
            }
        }
    }
}

