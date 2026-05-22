using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EventTicket.UI.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ApiService> _logger;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiService(HttpClient http, IHttpContextAccessor httpContextAccessor, ILogger<ApiService> logger)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        // Prefer the HttpOnly cookie; fall back to the "jwt_token" claim stored in the
        // ASP.NET auth cookie so API calls work even on plain HTTP (where Secure cookies
        // are not transmitted by the browser).
        var token = httpContextAccessor.HttpContext?.Request.Cookies["jwt_token"]
                 ?? httpContextAccessor.HttpContext?.User.FindFirst("jwt_token")?.Value;
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public void SetToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _http.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("GET {Endpoint} başarısız. StatusCode: {StatusCode}, Body: {Body}", endpoint, response.StatusCode, errorBody);
                LastError = $"{(int)response.StatusCode} - {errorBody}";
                return default;
            }
            LastError = null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "GET {Endpoint} sırasında bağlantı hatası oluştu.", endpoint);
            LastError = "Bağlantı hatası: " + ex.Message;
            return default;
        }
    }

    public async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("POST {Endpoint} başarısız. StatusCode: {StatusCode}, Body: {Body}", endpoint, response.StatusCode, errorBody);
                LastError = errorBody;
                return default;
            }
            LastError = null;
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseJson, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "POST {Endpoint} sırasında bağlantı hatası oluştu.", endpoint);
            LastError = "API'ye bağlanılamadı. Lütfen tekrar deneyin.";
            return default;
        }
    }

    public string? LastError { get; private set; }

    public async Task PutAsync(string endpoint, object data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(endpoint, content);
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("PUT {Endpoint} başarısız. StatusCode: {StatusCode}", endpoint, response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "PUT {Endpoint} sırasında bağlantı hatası oluştu.", endpoint);
        }
    }

    public async Task<T?> PatchAsync<T>(string endpoint)
    {
        try
        {
            var response = await _http.PatchAsync(endpoint, null);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("PATCH {Endpoint} başarısız. StatusCode: {StatusCode}", endpoint, response.StatusCode);
                return default;
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "PATCH {Endpoint} sırasında bağlantı hatası oluştu.", endpoint);
            return default;
        }
    }

    public async Task DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _http.DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("DELETE {Endpoint} başarısız. StatusCode: {StatusCode}", endpoint, response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "DELETE {Endpoint} sırasında bağlantı hatası oluştu.", endpoint);
        }
    }
}
