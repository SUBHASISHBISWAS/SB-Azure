
using Azure.Messaging.ServiceBus;
using AzureServiceBusMessages;
using Newtonsoft.Json;
using System;

string connectionString = "Endpoint=sb://appnamespace1000.servicebus.windows.net/;SharedAccessKeyName=appqueuepolicy;SharedAccessKey=B9/AR63bm9O/EjUYnQPhjRJO8TXqHpVC4HgUc1PTtcI=;EntityPath=appqueue";
string queueName = "appqueue";


ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
ServiceBusProcessor serviceBusProcessor= serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());

serviceBusProcessor.ProcessMessageAsync += ProcessMessage;
serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

await serviceBusProcessor.StartProcessingAsync();
Console.WriteLine("Waiting for messages");
Console.ReadKey();

await serviceBusProcessor.StopProcessingAsync();

await serviceBusProcessor.DisposeAsync();
await serviceBusClient.DisposeAsync();

static async Task ProcessMessage(ProcessMessageEventArgs messageEvent)
{
    Order order=JsonConvert.DeserializeObject<Order>(messageEvent.Message.Body.ToString());
    Console.WriteLine("Order Id {0}", order.OrderID);
    Console.WriteLine("Quantity {0}", order.Quantity);
    Console.WriteLine("Unit Price {0}", order.UnitPrice);

}

static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}