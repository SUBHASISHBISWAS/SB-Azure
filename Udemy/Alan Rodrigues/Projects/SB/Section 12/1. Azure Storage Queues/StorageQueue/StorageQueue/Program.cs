using Azure.Storage.Queues;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
string connectionString = "";
string queueName = "sbqueue";

SendMessage("Hello Subhasish    ");
void SendMessage(string message)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
	if (queueClient.Exists())
	{
		queueClient.SendMessage(message);
        Console.WriteLine("Message has been sent to Queue");
    }
}