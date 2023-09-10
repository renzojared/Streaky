using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Streaky.Movies.Services;

public class StorageFilesAzure : IStorageFiles
{
    private readonly string connectionString;

    public StorageFilesAzure(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("AzureStorage");
    }

    public async Task DeleteFile(string route, string container)
    {
        if (string.IsNullOrEmpty(route))
            return;

        var client = new BlobContainerClient(connectionString, container);
        await client.CreateIfNotExistsAsync();
        var file = Path.GetFileName(route);
        var blob = client.GetBlobClient(file);
        await blob.DeleteIfExistsAsync();
    }

    public async Task<string> EditFile(byte[] content, string extension, string container, string route, string contentType)
    {
        await DeleteFile(route, container);
        return await SaveFile(content, extension, container, contentType);
    }

    public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
    {
        var client = new BlobContainerClient(connectionString, container);
        await client.CreateIfNotExistsAsync();

        client.SetAccessPolicy(PublicAccessType.Blob);

        var fileName = $"{Guid.NewGuid()}.{extension}";
        var blob = client.GetBlobClient(fileName);

        var blobUploadOptions = new BlobUploadOptions();
        var blobHtppHeader= new BlobHttpHeaders();
        blobHtppHeader.ContentType = contentType;
        blobUploadOptions.HttpHeaders = blobHtppHeader;

        await blob.UploadAsync(new BinaryData(content), blobUploadOptions);

        return blob.Uri.ToString();
    }
}

