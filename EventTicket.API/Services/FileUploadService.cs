using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace EventTicket.API.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<FileUploadService> _logger;

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

    public FileUploadService(IWebHostEnvironment env, ILogger<FileUploadService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string folder = "uploads")
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            throw new InvalidOperationException($"İzin verilmeyen dosya türü: {ext}. İzin verilenler: jpg, jpeg, png, webp, gif");

        var uploadDir = Path.Combine(_env.WebRootPath ?? "wwwroot", folder);
        Directory.CreateDirectory(uploadDir);

        var uniqueName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadDir, uniqueName);

        await using var fs = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(fs);

        _logger.LogInformation("Dosya yüklendi: {Folder}/{File}", folder, uniqueName);
        return $"/{folder}/{uniqueName}";
    }

    public Task<IEnumerable<string>> ListAsync(string folder = "uploads")
    {
        var uploadDir = Path.Combine(_env.WebRootPath ?? "wwwroot", folder);
        if (!Directory.Exists(uploadDir))
            return Task.FromResult(Enumerable.Empty<string>());

        var files = Directory.GetFiles(uploadDir)
            .Where(f => AllowedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
            .Select(f => $"/{folder}/{Path.GetFileName(f)}");

        return Task.FromResult(files);
    }

    public Task<bool> DeleteAsync(string relativePath)
    {
        var fullPath = Path.Combine(
            _env.WebRootPath ?? "wwwroot",
            relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(fullPath))
            return Task.FromResult(false);

        File.Delete(fullPath);
        _logger.LogInformation("Dosya silindi: {Path}", relativePath);
        return Task.FromResult(true);
    }
}
