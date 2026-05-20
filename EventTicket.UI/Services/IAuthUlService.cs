using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IAuthUlService
{
    Task<AuthResponseViewModel?> LoginAsync(string email, string password);
    Task<AuthResponseViewModel?> RegisterAsync(
        string fullName, string username, string email, string password,
        string tcKimlikNo, DateTime dateOfBirth);
    Task<bool> DeleteAccountAsync();
    string? LastError { get; }
}
