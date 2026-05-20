using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface ITicketUlService
{
    Task<List<TicketViewModel>> GetMyTicketsAsync();
    Task<List<TicketViewModel>> PurchaseAsync(int eventId, int quantity);
}