using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace QueueFunction
{
    public class Queues
    {
        [FunctionName("GetMessages")]
        public void Run([QueueTrigger("appqueue", Connection = "connectionString")]Order order, ILogger log)
        {
            log.LogInformation("Order Id {0}",order.OrderID);
            log.LogInformation("Quantity {0}", order.Quantity);
        }
    }
}
