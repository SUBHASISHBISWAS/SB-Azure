
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore40404;AccountKey=4oz3Jb06afkZ5fznrdFtKyx4PI77jAFiba5DKtbfZLOsePVqhhgS3M4oWSA9iv0Xw9cTOVlYOjrGRkuH/ZMATw==;EndpointSuffix=core.windows.net";
string containerName = "data";
string blobName = "script.sql";
string filePath = "C:\\tmp1\\script.sql";

BlobClient blobClient=new BlobClient(connectionString, containerName, blobName);

await blobClient.DownloadToAsync(filePath);

Console.WriteLine("The blob is downloaded");



