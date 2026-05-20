using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class AuthUlService : IAuthUlService
{
    private readonly IApiService _api;

    public AuthUlService(IApiService api)
    {
        _api = api;
    }

    public string? LastError => _api.LastError;

    public async Task<AuthResponseViewModel?> LoginAsync(string email, string password)
    {
        return await _api.PostAsync<AuthResponseViewModel>("api/auth/login", new
        {
            email,
            password
        });
    }

    public async Task<AuthResponseViewModel?> RegisterAsync(
        string fullName, string username, string email, string password,
        string tcKimlikNo, DateTime dateOfBirth)
    {
        return await _api.PostAsync<AuthResponseViewModel>("api/auth/register", new
        {
            userName = username,
            fullName,
            email,
            password,
            tcKimlikNo,
            dateOfBirth
        });
    }

    public async Task<bool> DeleteAccountAsync()
    {
        try
        {
            await _api.DeleteAsync("api/auth/delete-account");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
