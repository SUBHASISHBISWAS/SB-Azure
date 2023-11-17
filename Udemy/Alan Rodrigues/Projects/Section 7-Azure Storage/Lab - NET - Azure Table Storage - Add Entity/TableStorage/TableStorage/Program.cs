
using Azure.Data.Tables;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore40404;AccountKey=4oz3Jb06afkZ5fznrdFtKyx4PI77jAFiba5DKtbfZLOsePVqhhgS3M4oWSA9iv0Xw9cTOVlYOjrGRkuH/ZMATw==;EndpointSuffix=core.windows.net";
string tableName = "Orders";

AddEntity("O1", "Mobile", 100);
AddEntity("O2", "Laptop", 50);
AddEntity("O3", "Desktop", 70);
AddEntity("O4", "Laptop", 200);

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