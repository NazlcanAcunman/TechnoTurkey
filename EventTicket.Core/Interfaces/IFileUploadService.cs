namespace EventTicket.Core.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string folder = "uploads");
    Task<IEnumerable<string>> ListAsync(string folder = "uploads");
    Task<bool> DeleteAsync(string relativePath);
}
