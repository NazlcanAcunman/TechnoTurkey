using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IAcademyUlService
{
    Task<List<AcademyContentViewModel>> GetAllAsync();
    Task<bool> CreateAsync(AcademyContentViewModel vm);
    Task<bool> UpdateAsync(int id, AcademyContentViewModel vm);
    Task<bool> DeleteAsync(int id);
}
