
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore40404;AccountKey=4oz3Jb06afkZ5fznrdFtKyx4PI77jAFiba5DKtbfZLOsePVqhhgS3M4oWSA9iv0Xw9cTOVlYOjrGRkuH/ZMATw==;EndpointSuffix=core.windows.net";
string containerName = "scripts";
string blobName = "script.sql";
string filePath = "C:\\tmp1\\script.sql";

BlobContainerClient blobServiceClient = new BlobContainerClient(connectionString,containerName);

BlobClient blobClient=blobServiceClient.GetBlobClient(blobName);
await blobClient.UploadAsync(filePath,true);

Console.WriteLine("Uploaded the blob");

