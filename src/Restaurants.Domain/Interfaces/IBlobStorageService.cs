namespace Restaurants.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    string GetBlobUri(string blobUrl);
}
