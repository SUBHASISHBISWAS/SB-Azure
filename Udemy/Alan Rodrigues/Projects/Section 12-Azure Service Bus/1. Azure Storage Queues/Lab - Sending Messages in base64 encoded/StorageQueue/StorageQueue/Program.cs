using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using StorageQueue;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore50505;AccountKey=4YjlzWJRRed7YnhbYJhVsd67ie3+aolDT9cOXZyB+hBFo4SUld8YyPpKsmbHfAQ8UTL5x5x6vZn1ntEexFeZWg==;EndpointSuffix=core.windows.net";
string queueName = "appqueue";


await SendMessage("O1", 100);
await SendMessage("O2", 200);
await SendMessage("O3", 300);

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

