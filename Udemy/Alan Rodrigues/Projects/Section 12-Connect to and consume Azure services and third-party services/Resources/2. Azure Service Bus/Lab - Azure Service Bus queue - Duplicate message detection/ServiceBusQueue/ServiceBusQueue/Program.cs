
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBusQueue;

string connectionString = "Endpoint=sb://appnamespace1000.servicebus.windows.net/;SharedAccessKeyName=orderqueuepolicy;SharedAccessKey=cLEen9cj5aMyv4kFmd5sNMKkkAtNwleKIbmvf5BLsz8=;EntityPath=orderqueue";
string queueName = "orderqueue";

List<Order> orders = new List<Order>()
{
        new Order(){OrderID="01",Quantity=100,UnitPrice=9.99F},
        new Order(){OrderID="02",Quantity=200,UnitPrice=10.99F},
        new Order(){OrderID="03",Quantity=300,UnitPrice=8.99F}

};

await SendMessage(orders);
//await GetProperties();
//await PeekMessage();

//await ReceiveMessages();


async Task SendMessage(List<Order> orders)
{
    ServiceBusClient serviceBusClient=new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender= serviceBusClient.CreateSender(queueName);

    ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    int messageId = 1;
    foreach(Order order in orders)
    {

        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(order));
        serviceBusMessage.ContentType = "application/json";
        serviceBusMessage.MessageId = messageId.ToString();
        messageId++;

        if (!serviceBusMessageBatch.TryAddMessage(
            serviceBusMessage))
        {
            throw new Exception("Error occured");
        }
            
    }
    Console.WriteLine("Sending messages");
    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);

    await serviceBusSender.DisposeAsync();
    await serviceBusClient.DisposeAsync();

}

async Task PeekMessage()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName,
        new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });

    ServiceBusReceivedMessage message=await serviceBusReceiver.ReceiveMessageAsync();
        
        Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
        Console.WriteLine("Order Id {0}", order.OrderID);
        Console.WriteLine("Quantity {0}", order.Quantity);
        Console.WriteLine("Unit Price {0}", order.UnitPrice);

    await serviceBusReceiver.CompleteMessageAsync(message);
}


async Task ReceiveMessages()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName,
        new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();

    await foreach (ServiceBusReceivedMessage message in messages)
    {
        Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
        Console.WriteLine("Order Id {0}", order.OrderID);
        Console.WriteLine("Quantity {0}", order.Quantity);
        Console.WriteLine("Unit Price {0}", order.UnitPrice);

    }
}

async Task GetProperties()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName,
        new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();

    await foreach (ServiceBusReceivedMessage message in messages)
    {
        Console.WriteLine("Message Id {0}",message.MessageId);
        Console.WriteLine("Sequence Number {0}", message.SequenceNumber);        
    }
    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}