using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace QueueFunction
{
    public class Queues
    {
        [FunctionName("GetMessages")]
        [return: Table("Orders", Connection = "connectionString")]
        public TableOrder Run([QueueTrigger("appqueue", Connection = "connectionString")]Order order, ILogger log)
        {
            TableOrder tableOrder = new TableOrder()
            {
                PartitionKey = order.OrderID,
                RowKey = order.Quantity.ToString()
            };

            log.LogInformation("Order written to table");
            return tableOrder;
        }
    }
}
