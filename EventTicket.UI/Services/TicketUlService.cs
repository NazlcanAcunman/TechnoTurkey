using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class TicketUlService : ITicketUlService
{
    private readonly IApiService _api;

    public TicketUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<TicketViewModel>> GetMyTicketsAsync()
    {
        return await _api.GetAsync<List<TicketViewModel>>("api/tickets/my")
               ?? new List<TicketViewModel>();
    }

    public async Task<List<TicketViewModel>> PurchaseAsync(int eventId, int quantity)
    {
        return await _api.PostAsync<List<TicketViewModel>>("api/tickets/purchase", new
        {
            eventId,
            quantity
        }) ?? new List<TicketViewModel>();
    }
}
