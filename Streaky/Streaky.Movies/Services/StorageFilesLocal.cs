namespace Streaky.Movies.Services;

public class StorageFilesLocal : IStorageFiles
{
    private readonly IWebHostEnvironment env; //ruta
    private readonly IHttpContextAccessor httpContextAccessor; //dominio web api

    public StorageFilesLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        this.env = env;
        this.httpContextAccessor = httpContextAccessor;
    }

    public Task DeleteFile(string route, string container)
    {
        if (route != null)
        {
            var nameFile = Path.GetFileName(route);
            string directoryFile = Path.Combine(env.WebRootPath, container, nameFile);

            if (File.Exists(directoryFile))
                File.Delete(directoryFile);
        }

        return Task.FromResult(0);
    }

    public async Task<string> EditFile(byte[] content, string extension, string container, string route, string contentType)
    {
        await DeleteFile(route, container);
        return await SaveFile(content, extension, container, contentType);
    }

    public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
    {
        var nameFile = $"{Guid.NewGuid()}{extension}";
        string folder = Path.Combine(env.WebRootPath, container);

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string route = Path.Combine(folder, nameFile);
        await File.WriteAllBytesAsync(route, content);

        var urlCurrent = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";

        var urlForDb = Path.Combine(urlCurrent, container, nameFile).Replace("\\", "/");

        return urlForDb;
    }
}

