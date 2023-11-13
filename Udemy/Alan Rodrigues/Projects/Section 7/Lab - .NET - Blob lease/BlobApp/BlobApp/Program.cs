
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=appstore40404;AccountKey=4oz3Jb06afkZ5fznrdFtKyx4PI77jAFiba5DKtbfZLOsePVqhhgS3M4oWSA9iv0Xw9cTOVlYOjrGRkuH/ZMATw==;EndpointSuffix=core.windows.net";
string containerName = "data";

await AcquireLease();

async Task AcquireLease()
    {

    string blobName = "script.sql";
    BlobClient blobClient = new BlobClient(connectionString,containerName, blobName);

    BlobLeaseClient blobLeaseClient = blobClient.GetBlobLeaseClient();
    TimeSpan leaseTime = new TimeSpan(0, 0, 1, 00);

    Response<BlobLease> response = await blobLeaseClient.AcquireAsync(leaseTime);


    Console.WriteLine("Lease id is {0}", response.Value.LeaseId);

    }




