
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore50505;AccountKey=4YjlzWJRRed7YnhbYJhVsd67ie3+aolDT9cOXZyB+hBFo4SUld8YyPpKsmbHfAQ8UTL5x5x6vZn1ntEexFeZWg==;EndpointSuffix=core.windows.net";
string queueName = "appqueue";

//SendMessage("Test Message 1");
//SendMessage("Test Message 2");
//PeekMessage();
ReceiveMessage();


void SendMessage(string message)
{
    QueueClient queueClient = new QueueClient(connectionString,queueName);

    if(queueClient.Exists())
    {
        queueClient.SendMessage(message);
        Console.WriteLine("The message has been sent");
    }
}

void PeekMessage()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    int maxMessages = 10;

    PeekedMessage[] peekMessages=queueClient.PeekMessages(maxMessages);
    Console.WriteLine("The messages in the queue are");
    foreach(var peekMessage in peekMessages)
    {
        Console.WriteLine(peekMessage.Body);
    }
}

void ReceiveMessage()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    int maxMessages = 10;

    QueueMessage[] queueMessages = queueClient.ReceiveMessages(maxMessages);
    Console.WriteLine("The messages in the queue are");
    foreach (var message in queueMessages)
    {
        Console.WriteLine(message.Body);
        queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
    }
}


