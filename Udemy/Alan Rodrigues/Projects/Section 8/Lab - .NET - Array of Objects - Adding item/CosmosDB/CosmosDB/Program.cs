
using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosDBEndpointUri = "https://appaccount10000.documents.azure.com:443/";
string cosmosDBKey = "LM13vhuJ7NvFTvReQJ1jILh4HbjgLwuFWMrPcHk2Q6qIZgZLoKkblDcHiCO34frtAtkzJVV5EOy1qHjwEsKHLQ==";
string databaseName = "appdb";
string containerName = "Customer";


await AddItem("C1", "CustomerA", "New York",
    new List<Order>() {
    new Order
    {
        orderId = "O1",
        category = "Laptop",
        quantity = 100
    },
     new Order
     {
         orderId = "O2",
         category = "Mobile",
         quantity = 10
     } });

await AddItem("C2", "CustomerB", "Chicago",
    new List<Order>() {
    new Order
    {
        orderId = "O3",
        category = "Laptop",
        quantity = 20
    }});


await AddItem("C3", "CustomerC", "Miami",
    new List<Order>() {
    new Order
    {
        orderId = "O4",
        category = "Desktop",
        quantity = 30
    },
     new Order
     {
         orderId = "O5",
         category = "Mobile",
         quantity = 40
     } });


async Task AddItem(string customerId, string customerName, string customerCity,List<Order> orders)
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    Customer customer = new Customer()
    {
        customerId = customerId,
        customerName = customerName,
        customerCity = customerCity,
        orders = orders
    };

    ItemResponse<Customer> response=await container.CreateItemAsync<Customer>(customer, new PartitionKey(customer.customerCity));

    Console.WriteLine("Added item with Customer Id {0}", customerId);
    Console.WriteLine("Request Units consumed {0}", response.RequestCharge);

}
