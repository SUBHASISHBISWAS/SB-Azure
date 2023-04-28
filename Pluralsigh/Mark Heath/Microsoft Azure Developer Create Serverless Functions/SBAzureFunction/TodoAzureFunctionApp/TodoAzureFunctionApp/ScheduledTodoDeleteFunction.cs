using System;
using Azure;
using System.Collections.Generic;

using Azure.Data.Tables;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TodoAzureFunctionApp
{
    public class ScheduledTodoDeleteFunction
    {
        private const string TableName = "todos";
        private const string PartitionKey = "TODO";

        [FunctionName("ScheduledTodoDeleteFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
           [Table(TableName, Connection = "AzureWebJobsStorage")] TableClient todoTable, ILogger log)
        {
            var pages = todoTable.QueryAsync<TodoTableEntity>().AsPages();

            var deleted = 0;
            await foreach (Page<TodoTableEntity> page in pages)
            {
                foreach (TodoTableEntity todo in page.Values)
                {
                    if (todo.IsCompleted)
                    {
                        try
                        {
                            var deleteResult = await todoTable.DeleteEntityAsync(PartitionKey, todo.RowKey, ETag.All);
                            deleted++;
                        }
                        catch (RequestFailedException e) when (e.Status == 404)
                        {
                           
                        }
                    }
                }

            }

            log.LogInformation($"Deleted {deleted} items at {DateTime.Now}");
        }
    }
}
