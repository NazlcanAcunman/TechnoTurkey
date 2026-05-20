using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class BannerUlService : IBannerUlService
{
    private readonly IApiService _api;

    public BannerUlService(IApiService api) => _api = api;

    public async Task<List<BannerViewModel>> GetActiveAsync()
        => await _api.GetAsync<List<BannerViewModel>>("api/banners") ?? new();

    public async Task<List<BannerViewModel>> GetAllAsync()
        => await _api.GetAsync<List<BannerViewModel>>("api/banners/all") ?? new();

    public async Task<BannerViewModel?> GetByIdAsync(int id)
        => await _api.GetAsync<BannerViewModel>($"api/banners/{id}");

    public async Task CreateAsync(BannerViewModel model)
    {
        var result = await _api.PostAsync<object>("api/banners", new
        {
            model.Title,
            model.SubTitle,
            model.DateText,
            model.Venue,
            model.ImageUrl,
            model.SearchQuery,
            model.TicketUrl,
            model.Type,
            model.SortOrder,
            model.IsActive
        });
        if (result == null && _api.LastError != null)
            throw new Exception(_api.LastError);
    }

    public async Task UpdateAsync(int id, BannerViewModel model)
        => await _api.PutAsync($"api/banners/{id}", new
        {
            model.Title,
            model.SubTitle,
            model.DateText,
            model.Venue,
            model.ImageUrl,
            model.SearchQuery,
            model.TicketUrl,
            model.Type,
            model.SortOrder,
            model.IsActive
        });

    public async Task DeleteAsync(int id)
        => await _api.DeleteAsync($"api/banners/{id}");
}
