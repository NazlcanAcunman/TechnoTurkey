using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IBannerService
{
    Task<IEnumerable<BannerResponseDto>> GetActiveAsync();
    Task<IEnumerable<BannerResponseDto>> GetAllAsync();
    Task<BannerResponseDto?> GetByIdAsync(int id);
    Task<BannerResponseDto> CreateAsync(CreateBannerDto dto);
    Task UpdateAsync(int id, UpdateBannerDto dto);
    Task DeleteAsync(int id);
}
