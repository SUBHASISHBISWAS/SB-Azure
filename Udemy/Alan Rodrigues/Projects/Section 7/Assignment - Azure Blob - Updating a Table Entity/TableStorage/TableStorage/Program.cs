
using Azure.Data.Tables;
using Azure;
using TableStorage;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore40404;AccountKey=4oz3Jb06afkZ5fznrdFtKyx4PI77jAFiba5DKtbfZLOsePVqhhgS3M4oWSA9iv0Xw9cTOVlYOjrGRkuH/ZMATw==;EndpointSuffix=core.windows.net";
string tableName = "Orders";

UpdateEntity("Desktop", "O3",300);

void AddEntity(string orderID,string category,int quantity)
{
    TableClient tableClient = new TableClient(connectionString, tableName);

    TableEntity tableEntity = new TableEntity(category, orderID)
    {
        {"quantity",quantity}
    };

    tableClient.AddEntity(tableEntity);
    Console.WriteLine("Adding Entity with Partition Key {0} and Row Key {1}", category, orderID);
}

void QueryEntity(string category)
{
    TableClient tableClient = new TableClient(connectionString, tableName);

    Pageable<TableEntity> queryResults = tableClient.Query<TableEntity>(entity => entity.PartitionKey == category);

    Console.WriteLine("The Entities within the partition {0}", category);
    foreach (TableEntity tableEntity in queryResults)
    {
        

        Console.WriteLine("Order ID {0}",tableEntity.RowKey);
        Console.WriteLine("Quantity {0}", tableEntity.GetInt32("quantity"));

    }
}

void DeleteEntity(string category,string orderID)
{
    TableClient tableClient = new TableClient(connectionString, tableName);
    tableClient.DeleteEntity(category, orderID);
    Console.WriteLine("Entity with Partition Key {0} and Row Key {1} deleted", category, orderID);
}

void UpdateEntity(string category, string orderID, int quantity)
{
    // Let's first get the entity that we want to update
    TableClient tableClient = new TableClient(connectionString, tableName);
    Order order = tableClient.GetEntity<Order>(category, orderID);
    order.quantity = quantity;

    tableClient.UpdateEntity<Order>(order,ifMatch:ETag.All,TableUpdateMode.Replace);

    Console.WriteLine("Entity updated");
    }