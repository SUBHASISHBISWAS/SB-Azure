using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusQueueFunction
{
    public static class GetMessages
    {
        [FunctionName("GetMessages")]
        public static void Run([ServiceBusTrigger("orders", Connection = "connectionString")]Message QueueMessage, ILogger log)
        {
            log.LogInformation("Message Body {0}",Encoding.UTF8.GetString(QueueMessage.Body));
            log.LogInformation("Sequence Number {0}",QueueMessage.SystemProperties.SequenceNumber);
        }
    }
}
