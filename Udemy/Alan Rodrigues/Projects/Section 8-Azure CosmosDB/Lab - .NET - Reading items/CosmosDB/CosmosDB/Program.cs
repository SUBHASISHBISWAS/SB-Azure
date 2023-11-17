
using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosDBEndpointUri = "https://appaccount10000.documents.azure.com:443/";
string cosmosDBKey = "LM13vhuJ7NvFTvReQJ1jILh4HbjgLwuFWMrPcHk2Q6qIZgZLoKkblDcHiCO34frtAtkzJVV5EOy1qHjwEsKHLQ==";
string databaseName = "appdb";
string containerName = "Orders";


await ReadItems();

async Task ReadItems()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string sqlQuery = "SELECT o.orderId,o.category,o.quantity FROM Orders o";

    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
    using FeedIterator<Order> feedIterator = container.GetItemQueryIterator<Order>(queryDefinition);

    while(feedIterator.HasMoreResults)
    {
        FeedResponse<Order> respose = await feedIterator.ReadNextAsync();
        foreach(Order order in respose)
        {
            Console.WriteLine("Order Id {0}", order.orderId);
            Console.WriteLine("Category {0}", order.category);
            Console.WriteLine("Quantity {0}", order.quantity);
        }
    }
}
