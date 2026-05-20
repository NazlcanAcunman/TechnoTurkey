using EventTicket.Core.Interfaces;
using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class AcademyUlService : IAcademyUlService
{
    private readonly IApiService _api;

    public AcademyUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<AcademyContentViewModel>> GetAllAsync()
    {
        return await _api.GetAsync<List<AcademyContentViewModel>>("api/academy")
               ?? new List<AcademyContentViewModel>();
    }

    public async Task<bool> CreateAsync(AcademyContentViewModel vm)
    {
        var result = await _api.PostAsync<AcademyContentViewModel>("api/academy", vm);
        return result != null;
    }

    public async Task<bool> UpdateAsync(int id, AcademyContentViewModel vm)
    {
        await _api.PutAsync($"api/academy/{id}", vm);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _api.DeleteAsync($"api/academy/{id}");
        return true;
    }
}
