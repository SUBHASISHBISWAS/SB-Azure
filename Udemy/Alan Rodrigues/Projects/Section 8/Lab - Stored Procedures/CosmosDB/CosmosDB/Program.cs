
using Microsoft.Azure.Cosmos;

string cosmosDBEndpointUri = "https://appaccount10000.documents.azure.com:443/";
string cosmosDBKey = "3rrPVN8LI4e5cgrSa9PBB2IdaqYl0z2uYmYMb7Y7hRbyME6wn84iYtMaiHzOhI0TtyisPCdkF5zbQeQEpXHZ5Q==";
string databaseName = "appdb";
string containerName = "Orders";

// await CreateDatabase("appdb");
CosmosClient cosmosClient;
cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);

Container container = cosmosClient.GetContainer(databaseName, containerName);

await CallStoredProcedure();

async Task CallStoredProcedure()
{
    
    var result = await container.Scripts.ExecuteStoredProcedureAsync<string>("Display", new PartitionKey(""), null);

    Console.WriteLine(result);

    
}

