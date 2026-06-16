using EventTicket.Core.Common.Results;
using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IGuestlistService
{
    Task<DataResult<GuestlistRequestResponseDto>> CreateAsync(CreateGuestlistRequestDto dto, string userId);
    Task<DataResult<List<GuestlistRequestResponseDto>>> CreateBulkAsync(List<CreateGuestlistRequestDto> dtos, string userId);
    Task<DataResult<List<GuestlistRequestResponseDto>>> GetByEventAsync(int eventId);
    Task<DataResult<List<GuestlistRequestResponseDto>>> GetByUserAsync(string userId);
    Task<DataResult<GuestlistRequestResponseDto>> UpdateStatusAsync(int id, UpdateGuestlistStatusDto dto);
    Task<DataResult<GuestlistStatsDto>> GetStatsAsync(int eventId);
    Task<DataResult<bool>> DeleteAsync(int id);
}
