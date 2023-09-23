using Azure.Storage.Queues;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore50505;AccountKey=4YjlzWJRRed7YnhbYJhVsd67ie3+aolDT9cOXZyB+hBFo4SUld8YyPpKsmbHfAQ8UTL5x5x6vZn1ntEexFeZWg==;EndpointSuffix=core.windows.net";
string queueName = "appqueue";

sendMessage("Test Message 1");
sendMessage("Test Message 2");

void sendMessage(string message)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    if (queueClient.Exists())
    {
       queueClient.SendMessage(message);
        Console.WriteLine("Message sent {0}", message);
    }
}
