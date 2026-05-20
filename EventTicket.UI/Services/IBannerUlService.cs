using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IBannerUlService
{
    Task<List<BannerViewModel>> GetActiveAsync();
    Task<List<BannerViewModel>> GetAllAsync();
    Task<BannerViewModel?> GetByIdAsync(int id);
    Task CreateAsync(BannerViewModel model);
    Task UpdateAsync(int id, BannerViewModel model);
    Task DeleteAsync(int id);
}
