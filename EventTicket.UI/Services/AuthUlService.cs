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

    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            await _api.PostAsync<object>("api/auth/change-password", new
            {
                currentPassword,
                newPassword
            });
            return _api.LastError == null;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateUsernameAsync(string newUsername)
    {
        try
        {
            await _api.PutAsync("api/auth/update-username", new { username = newUsername });
            return _api.LastError == null;
        }
        catch { return false; }
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
