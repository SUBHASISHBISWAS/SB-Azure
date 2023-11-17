
using Azure.Data.Tables;
using Azure;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=vmstore5677676;AccountKey=cHp2lkxmpth+XkH2gOh+luJP2x0l9NUcHXR4nx5dmgDr6VSHQI8QwoICjub3FvynjNJtnGUZl9e5PFY3d2oq/w==;EndpointSuffix=core.windows.net";
string tableName = "Orders";

/*
AddEntity("O1", "Mobile", 100);
AddEntity("O2", "Laptop", 50);
AddEntity("O3", "Desktop", 70);
AddEntity("O4", "Laptop", 200);*/

//QueryEntity("Laptop");

DeleteEntity("Laptop", "O2");

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