
using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosDBEndpointUri = "https://appaccount10000.documents.azure.com:443/";
string cosmosDBKey = "LM13vhuJ7NvFTvReQJ1jILh4HbjgLwuFWMrPcHk2Q6qIZgZLoKkblDcHiCO34frtAtkzJVV5EOy1qHjwEsKHLQ==";
string databaseName = "appdb";
string containerName = "Customer";

await ReadItems();

async Task ReadItems()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string sqlQuery = "SELECT c.id,c.customerName,c.customerCity,c.orders AS Orders FROM Customer c";

    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
    using FeedIterator<Customer> feedIterator = container.GetItemQueryIterator<Customer>(queryDefinition);

    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Customer> respose = await feedIterator.ReadNextAsync();
        foreach (Customer customer in respose)
        {
            Console.WriteLine("Customer Id {0}", customer.customerId);
            Console.WriteLine("Customer Name {0}", customer.customerName);
            Console.WriteLine("Customer City {0}", customer.customerCity);
            foreach(Order order in customer.orders)
            {
                Console.WriteLine("Order ID {0}", order.orderId);
                Console.WriteLine("Category  {0}", order.category);
                Console.WriteLine("Quantity {0}", order.quantity);
            }
        }
    }
}
