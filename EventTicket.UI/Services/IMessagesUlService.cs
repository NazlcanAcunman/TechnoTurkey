using EventTicket.UI.ViewModels.Admin;

namespace EventTicket.UI.Services;

public interface IMessagesUlService
{
    Task<List<MessageViewModel>> GetAllAsync();
    Task MarkAsReadAsync(int id);
    Task DeleteAsync(int id);
    Task SendAsync(string name, string email, string subject, string message);
}