using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=saccalanrodrigues;AccountKey=ow7WEYyFRsfcy9imalNcMec2XBNtIuFxO5V4PyGmaT7avoXSNHU4mVBoLUKan0W2/WXWwS3ZOyaz+AStHdcwWw==;EndpointSuffix=core.windows.net";
string queueName = "sbqueue";

//SendSimpleMessage("Hello Subhasish");

//PeekSimpleMessage();

//Console.WriteLine("Queue Length:: {0}",GetQueueLength());

ReceiveSimpleMessage();

void SendSimpleMessage(string message)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
	if (queueClient.Exists())
	{
		queueClient.SendMessage(message);
        Console.WriteLine("Message has been sent to Queue");
    }
}

void PeekSimpleMessage()
{
    QueueClient queueClient =new QueueClient(connectionString, queueName);
    int maxMessage = 10;
    if (queueClient.Exists())
    {
        PeekedMessage[] peekedMessages=queueClient.PeekMessages(maxMessage);
        Console.WriteLine("In Queue Below message are Present");
        foreach (var peekedMessage in peekedMessages) { 
            Console.WriteLine(peekedMessage.Body); 
        }
    }
}

void ReceiveSimpleMessage()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    int maxMessage = 10;
    if (queueClient.Exists())
    {
        QueueMessage[] queueMessages = queueClient.ReceiveMessages(maxMessage);
        Console.WriteLine("In Queue Below message are Present");
        foreach (var queueMessage in queueMessages)
        {
            Console.WriteLine(queueMessage.Body);
            queueClient.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
        }
    }
}

int GetQueueLength()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    if (queueClient.Exists())
    {
        QueueProperties properties = queueClient.GetProperties();
        return properties.ApproximateMessagesCount;
    }
    return 0;
}

