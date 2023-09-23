
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBusQueue;

string connectionString = "Endpoint=sb://appnamespace1000.servicebus.windows.net/;SharedAccessKeyName=appqueuepolicy;SharedAccessKey=B9/AR63bm9O/EjUYnQPhjRJO8TXqHpVC4HgUc1PTtcI=;EntityPath=appqueue";
string queueName = "appqueue";

List<Order> orders = new List<Order>()
{
        new Order(){OrderID="01",Quantity=100,UnitPrice=9.99F},
        new Order(){OrderID="02",Quantity=200,UnitPrice=10.99F},
        new Order(){OrderID="03",Quantity=300,UnitPrice=8.99F}

};

await SendMessage(orders);


async Task SendMessage(List<Order> orders)
{
    ServiceBusClient serviceBusClient=new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender= serviceBusClient.CreateSender(queueName);

    ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    foreach(Order order in orders)
    {

        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(order));
        serviceBusMessage.ContentType = "application/json";
        if (!serviceBusMessageBatch.TryAddMessage(
            serviceBusMessage))
        {
            throw new Exception("Error occured");
        }
            
    }
    Console.WriteLine("Sending messages");
    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);
}