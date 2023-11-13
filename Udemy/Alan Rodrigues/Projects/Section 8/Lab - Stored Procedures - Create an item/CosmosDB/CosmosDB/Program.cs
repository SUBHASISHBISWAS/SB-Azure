
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

    dynamic[] orderItems = new dynamic[]
{
    new {
        id = Guid.NewGuid().ToString(),
        orderId = "01",
        category  = "Laptop",
        quantity  = 100
    },
    new {
        id = Guid.NewGuid().ToString(),
        orderId = "02",
        category  = "Laptop",
        quantity  = 200
    },
    new {
        id = Guid.NewGuid().ToString(),
        orderId = "03",
        category  = "Laptop",
        quantity  = 75
    },
};

    var result = await container.Scripts.ExecuteStoredProcedureAsync<string>("createItems", new PartitionKey("Laptop"), new[] { orderItems});

    Console.WriteLine(result);

    
}

