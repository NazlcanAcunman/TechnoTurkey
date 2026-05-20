using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IVenueService
{
    Task<IEnumerable<VenueResponseDto>> GetAllApprovedAsync();
    Task<IEnumerable<VenueResponseDto>> GetAllPendingAsync();
    Task<IEnumerable<VenueResponseDto>> GetByAdminAsync(string adminUserId);  
    Task<VenueResponseDto?> GetByIdAsync(int id);
    Task<VenueResponseDto> CreateAsync(CreateVenueDto dto, string adminUserId);
    Task UpdateAsync(int id, UpdateVenueDto dto, string adminUserId, bool isSuperAdmin);
    Task DeleteAsync(int id);
    Task ApproveAsync(int id);
    Task RejectAsync(int id);   
}