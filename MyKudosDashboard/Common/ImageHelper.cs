using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;

namespace MyKudosDashboard.Common;


public class ImageHelper : IImageHelper
{
    private string _storage_ConnectionString;
    private string _storage_ContainerName;


    public ImageHelper(IConfiguration configuration)
    {
        _storage_ConnectionString = configuration["Storage_ConnectionString"];
        _storage_ContainerName = configuration["Storage_ContainerName"];
    }

    public string GetImageUrl(string imageName)
    {

        // Extract the account name and account key from the connection string
        var connectionStringParts = _storage_ConnectionString.Split(';');
        string accountName = connectionStringParts.FirstOrDefault(part => part.StartsWith("AccountName="))?.Substring("AccountName=".Length);
        string accountKey = connectionStringParts.FirstOrDefault(part => part.StartsWith("AccountKey="))?.Substring("AccountKey=".Length);


        BlobServiceClient blobServiceClient = new BlobServiceClient(_storage_ConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_storage_ContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(imageName);

        // Generate a SAS token
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerClient.Name,
            BlobName = blobClient.Name,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1), // Set the expiration time as needed
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasToken = sasBuilder.ToSasQueryParameters(
                            new StorageSharedKeyCredential(
                                    accountName, accountKey)).ToString();

        // Return the URL with the SAS token
        return $"{blobClient.Uri}?{sasToken}";
    }



    public async Task UploadImage(string fileName, Stream stream)
    {

        // Create a BlobServiceClient
        BlobServiceClient blobServiceClient = new BlobServiceClient(_storage_ConnectionString);

        // Get a reference to the container
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_storage_ContainerName);

        // Create the container if it doesn't exist
        await containerClient.CreateIfNotExistsAsync();

        // Get a reference to the blob (the uploaded file)
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        // Upload the file to the blob
        await blobClient.UploadAsync(stream, overwrite: true);
    }
}
