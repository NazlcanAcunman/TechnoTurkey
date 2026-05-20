using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IFavoriteService
{
    Task<IEnumerable<FavoriteResponseDto>> GetMyFavoritesAsync(string userId);
    Task<FavoriteResponseDto> AddAsync(AddFavoriteDto dto, string userId);
    Task RemoveAsync(int id, string userId);
}