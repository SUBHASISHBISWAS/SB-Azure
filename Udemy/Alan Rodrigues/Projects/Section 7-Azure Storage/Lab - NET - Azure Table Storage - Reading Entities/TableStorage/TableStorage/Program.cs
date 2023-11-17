
using Azure;
using Azure.Data.Tables;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore40404;AccountKey=4oz3Jb06afkZ5fznrdFtKyx4PI77jAFiba5DKtbfZLOsePVqhhgS3M4oWSA9iv0Xw9cTOVlYOjrGRkuH/ZMATw==;EndpointSuffix=core.windows.net";
string tableName = "Orders";

QueryEntity("Laptop");

void AddEntity(string orderID,string category,int quantity)
{
    TableClient tableClient = new TableClient(connectionString, tableName);

    TableEntity tableEntity = new TableEntity(category, orderID)
    {
        {"quantity",quantity}
    };

    tableClient.AddEntity(tableEntity);
    Console.WriteLine("Added Entity with order ID {0}",orderID);
}

void QueryEntity(string category)
{
    TableClient tableClient = new TableClient(connectionString, tableName);

    Pageable<TableEntity> results=tableClient.Query<TableEntity>(entity=>entity.PartitionKey == category);

    foreach(TableEntity tableEntity in results)
    {
        Console.WriteLine("Order Id {0}", tableEntity.RowKey);
        Console.WriteLine("Quantity is {0}", tableEntity.GetInt32("quantity"));

    }
}