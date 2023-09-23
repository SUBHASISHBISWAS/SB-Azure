using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore50505;AccountKey=4YjlzWJRRed7YnhbYJhVsd67ie3+aolDT9cOXZyB+hBFo4SUld8YyPpKsmbHfAQ8UTL5x5x6vZn1ntEexFeZWg==;EndpointSuffix=core.windows.net";
string queueName = "appqueue";

Console.WriteLine("The number of messages in the queue {0}", GetQueueLength());

void SendMessage(string message)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    if (queueClient.Exists())
    {
       queueClient.SendMessage(message);
        Console.WriteLine("Message sent {0}", message);
    }
}

void PeekMessage()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    int maxMessages = 10;

    if (queueClient.Exists())
    {
        PeekedMessage[] peekedMessages = queueClient.PeekMessages(maxMessages);
        Console.WriteLine("The messages in the queue");
        foreach(var message in peekedMessages)
        {
            Console.WriteLine(message.Body);
        }
    }
}

void ReceiveMessage()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    int maxMessages = 10;

    if (queueClient.Exists())
    {
        QueueMessage[] queueMessages = queueClient.ReceiveMessages(maxMessages);
        Console.WriteLine("The messages in the queue");
        foreach (var message in queueMessages)
        {
            Console.WriteLine(message.Body);
        }
    }
}

int GetQueueLength()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    if (queueClient.Exists())
    {
        QueueProperties queueProperties = queueClient.GetProperties();
        return queueProperties.ApproximateMessagesCount;
    }

    return 0;
}
