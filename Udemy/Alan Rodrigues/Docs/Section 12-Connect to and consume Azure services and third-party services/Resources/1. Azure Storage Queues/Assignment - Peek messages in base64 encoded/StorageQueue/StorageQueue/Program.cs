using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using StorageQueue;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore50505;AccountKey=4YjlzWJRRed7YnhbYJhVsd67ie3+aolDT9cOXZyB+hBFo4SUld8YyPpKsmbHfAQ8UTL5x5x6vZn1ntEexFeZWg==;EndpointSuffix=core.windows.net";
string queueName = "appqueue";


await PeekMessages();

async Task SendMessage(string orderid,int quantity)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    if (queueClient.Exists())
    {
       Order order=new Order {OrderID = orderid,Quantity=quantity};
        var jsonObject = JsonConvert.SerializeObject(order);
        var bytes= System.Text.Encoding.UTF8.GetBytes(jsonObject);
        var message= System.Convert.ToBase64String(bytes);  
       await queueClient.SendMessageAsync(message);
       Console.WriteLine("Order Id {0} sent", orderid);
    }
}


async Task PeekMessages()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    int maxMessages = 10;

    if (queueClient.Exists())
    {
        PeekedMessage[] peekedMessages = await queueClient.PeekMessagesAsync(maxMessages);
        Console.WriteLine("The orders in the queue");
        foreach (var message in peekedMessages)
        {
            var base64String = System.Convert.FromBase64String(message.Body.ToString());
            var stringData = System.Text.Encoding.UTF8.GetString(base64String);
            Order order = JsonConvert.DeserializeObject<Order>(stringData);
            Console.WriteLine("Order Id {0}", order.OrderID);
            Console.WriteLine("Order Quantity {0}", order.Quantity);
        }
    }
}

