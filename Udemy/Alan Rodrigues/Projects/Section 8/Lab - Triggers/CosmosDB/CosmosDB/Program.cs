using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosEndpointUri = "https://appaccount10000.documents.azure.com:443/";
string cosmosDBKey = "3rrPVN8LI4e5cgrSa9PBB2IdaqYl0z2uYmYMb7Y7hRbyME6wn84iYtMaiHzOhI0TtyisPCdkF5zbQeQEpXHZ5Q==";
string databaseName = "appdb";
string containerName = "Orders";


await CreateItem();

async Task CreateItem()
{
    CosmosClient cosmosClient;
    cosmosClient=new CosmosClient(cosmosEndpointUri,cosmosDBKey);

    Container container= cosmosClient.GetContainer(databaseName,containerName);

    dynamic orderItem =
        new
        {
            id=Guid.NewGuid().ToString(),
            orderId="O1",
            category="Laptop"
        };

    await container.CreateItemAsync(orderItem, null, new ItemRequestOptions { PreTriggers = new List<string> { "validateItem" } });

    Console.WriteLine("Item has been inserted");
    
}