using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infastructure.Configuration;

namespace Restaurants.Infastructure.Storage;

public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOptions) : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettingsOptions.Value;
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
       var blobServiceClient=  new BlobServiceClient(_blobStorageSettings.ConnectionString);
       var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);
       var blobClient = containerClient.GetBlobClient(fileName);
       await blobClient.UploadAsync(fileStream);
       var blobUri= blobClient.Uri.ToString();
       return blobUri;
    }
    public string? GetBlobUri(string blobUrl)
    {
        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = _blobStorageSettings.LogosContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName = GetNameFromUrl(blobUrl),

        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var sasToken = sasBuilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(
            blobServiceClient.AccountName,_blobStorageSettings.AccountKey)).ToString();

        return $"{blobUrl}?{sasToken}";
    }
    private string ? GetNameFromUrl(string url)
    {
        var uri = new Uri(url);
        return uri.Segments.Last();
    }
}
