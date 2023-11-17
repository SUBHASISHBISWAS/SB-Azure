
using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosDBEndpointUri = "https://appaccount10000.documents.azure.com:443/";
string cosmosDBKey = "LM13vhuJ7NvFTvReQJ1jILh4HbjgLwuFWMrPcHk2Q6qIZgZLoKkblDcHiCO34frtAtkzJVV5EOy1qHjwEsKHLQ==";
string databaseName = "appdb";
string containerName = "Customer";

await ReplaceItems();

async Task ReplaceItems()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string customerId = "C2";
    string sqlQuery = $"SELECT c.id,c.customerCity FROM Customer c WHERE c.id='{customerId}'";

    string customerCity = "";

    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
    using FeedIterator<Customer> feedIterator = container.GetItemQueryIterator<Customer>(queryDefinition);

    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Customer> respose = await feedIterator.ReadNextAsync();
        foreach (Customer customer in respose)
        {
            customerCity = customer.customerCity;
        }
    }


    // Get the specific item first
    ItemResponse<Customer> customerResponse = await container.ReadItemAsync<Customer>(customerId, new PartitionKey(customerCity));

    var item = customerResponse.Resource;    
    
    item.orders.Add(new Order { orderId="O6",category="Desktop",quantity=300});

    // Now let's replace the item
    await container.ReplaceItemAsync<Customer>(item, customerId, new PartitionKey(customerCity));
    Console.WriteLine("Item is updated");
}