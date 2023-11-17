
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore40404;AccountKey=4oz3Jb06afkZ5fznrdFtKyx4PI77jAFiba5DKtbfZLOsePVqhhgS3M4oWSA9iv0Xw9cTOVlYOjrGRkuH/ZMATw==;EndpointSuffix=core.windows.net";
string containerName = "data";

BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString,containerName);

await foreach(BlobItem blobItem in blobContainerClient.GetBlobsAsync())
{
    Console.WriteLine("The Blob Name is {0}",blobItem.Name);
    Console.WriteLine("The Blob Size is {0}", blobItem.Properties.ContentLength);
}


