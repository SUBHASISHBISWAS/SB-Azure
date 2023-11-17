using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBusTopicReceiver;

string connectionString = "Endpoint=sb://appnamespace1000.servicebus.windows.net/;SharedAccessKeyName=ListenPolicy;SharedAccessKey=neRt9/m4yR/g9DRI2Zb8HZfOW/L8Td06p/YV4QKkCv8=;EntityPath=apptopic";
string topicName = "apptopic";
string subscriptionName = "SubscriptionA";

await ReceiveMessages();

async Task ReceiveMessages()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(topicName, subscriptionName,
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

