namespace MinimalAPIsTopLearn.Services;

public class FileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }
    public Task Delete(string? route, string container)
    {
        if (string.IsNullOrEmpty(route))
        {
            return Task.CompletedTask;
        }
        var fileName = Path.GetFileName(route);
        var fileDirectory = Path.Combine(_env.WebRootPath, container, fileName);

        if (File.Exists(fileDirectory))
        {
            File.Delete(fileDirectory);
        }
        return Task.CompletedTask;
    }

    public async Task<string> Store(string container, IFormFile file)
    {
        var extention = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extention}";
        string folder = Path.Combine(_env.WebRootPath, container);

        if (Directory.Exists(folder) == false)
        {
            Directory.CreateDirectory(folder);
        }

        string route = Path.Combine(folder, fileName);

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var content = ms.ToArray();
        await File.WriteAllBytesAsync(route, content);

        var scheme = _httpContextAccessor.HttpContext!.Request.Scheme;
        var host = _httpContextAccessor.HttpContext!.Request.Host;
        var url = $"{scheme}://{host}";

        var fileUrl = Path.Combine(url, container, fileName).Replace("\\", "/");
        return fileUrl;
    }
}
