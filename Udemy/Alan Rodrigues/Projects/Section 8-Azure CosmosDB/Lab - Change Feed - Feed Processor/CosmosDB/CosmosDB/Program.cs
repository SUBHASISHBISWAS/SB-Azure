
using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosDBEndpointUri = "https://appaccount10000.documents.azure.com:443/";
string cosmosDBKey = "3rrPVN8LI4e5cgrSa9PBB2IdaqYl0z2uYmYMb7Y7hRbyME6wn84iYtMaiHzOhI0TtyisPCdkF5zbQeQEpXHZ5Q==";
string databaseName = "appdb";
string sourcecontainerName = "Orders";
string leasecontainerName = "lease";


await StartChangeProcessor();

async Task StartChangeProcessor()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);

    Container leaseContainer = cosmosClient.GetContainer(databaseName, leasecontainerName);
    
    ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(databaseName, sourcecontainerName)
        .GetChangeFeedProcessorBuilder<Order>(processorName:"ManageChanges", onChangesDelegate: ManageChanges)
        .WithInstanceName("appHost")
        .WithLeaseContainer(leaseContainer)
        .Build();

    Console.WriteLine("Starting the Change Feed Processor");
    await changeFeedProcessor.StartAsync();
    Console.Read();
    await changeFeedProcessor.StopAsync();
}



static async Task ManageChanges(
    ChangeFeedProcessorContext context,
    IReadOnlyCollection<Order> itemCollection,
    CancellationToken cancellationToken)
{
    foreach(Order item in itemCollection)
    {
        Console.WriteLine("Id {0}",item.id);
        Console.WriteLine("Order Id {0}", item.orderId);
        Console.WriteLine("Creation Time {0}", item.creationTime);
        await Task.Delay(10);
    }
}