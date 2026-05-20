using EventTicket.UI.ViewModels.Admin;

namespace EventTicket.UI.Services;

public class MessagesUlService : IMessagesUlService
{
    private readonly IApiService _api;

    public MessagesUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<MessageViewModel>> GetAllAsync()
        => await _api.GetAsync<List<MessageViewModel>>("api/admin/contact")
           ?? new List<MessageViewModel>();

    public async Task MarkAsReadAsync(int id)
        => await _api.PatchAsync<object>($"api/admin/contact/{id}/read");

    public async Task DeleteAsync(int id)
        => await _api.DeleteAsync($"api/admin/contact/{id}");

    public async Task SendAsync(string name, string email, string subject, string message)
        => await _api.PostAsync<object>("api/contact", new { name, email, subject, message });
}