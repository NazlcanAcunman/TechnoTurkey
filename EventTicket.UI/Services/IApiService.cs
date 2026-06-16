namespace EventTicket.UI.Services;

public interface IApiService
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object data);
    Task<T?> PatchAsync<T>(string endpoint);
    Task PutAsync(string endpoint, object data);
    Task<T?> PutAsync<T>(string endpoint, object data);
    Task DeleteAsync(string endpoint);
    void SetToken(string token);
    string? LastError { get; }
}