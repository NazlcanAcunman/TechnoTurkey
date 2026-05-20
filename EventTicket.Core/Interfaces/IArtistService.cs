using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IArtistService
{
    Task<IEnumerable<ArtistResponseDto>> GetAllApprovedAsync();
    Task<IEnumerable<ArtistResponseDto>> GetAllPendingAsync();
    Task<ArtistResponseDto?> GetByIdAsync(int id);
    Task<ArtistResponseDto> CreateAsync(CreateArtistDto dto);
    Task UpdateAsync(int id, UpdateArtistDto dto);
    Task DeleteAsync(int id);
    Task ApproveAsync(int id);
    Task RejectAsync(int id);   
}