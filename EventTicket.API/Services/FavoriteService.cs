using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IRepository<Favorite> _favoriteRepo;

    public FavoriteService(IRepository<Favorite> favoriteRepo)
    {
        _favoriteRepo = favoriteRepo;
    }

    public async Task<IEnumerable<FavoriteResponseDto>> GetMyFavoritesAsync(string userId)
    {
        var favorites = await _favoriteRepo.FindWithIncludesAsync(
            f => f.UserId == userId && !f.IsDeleted,
            f => f.Venue!,
            f => f.Artist!);

        return favorites.Select(f => new FavoriteResponseDto
        {
            Id = f.Id,
            VenueId = f.VenueId,
            VenueName = f.Venue?.Name ?? "",
            ArtistId = f.ArtistId,
            ArtistName = f.Artist?.Name ?? ""
        });
    }

    public async Task<FavoriteResponseDto> AddAsync(AddFavoriteDto dto, string userId)
    {
        if (dto.VenueId == null && dto.ArtistId == null)
            throw new ArgumentException("Mekan veya sanatçı belirtmelisiniz.");

        if (dto.VenueId.HasValue && dto.ArtistId.HasValue)
            throw new ArgumentException("Aynı anda hem mekan hem sanatçı favorilenемez.");

        var existing = await _favoriteRepo.FindAsync(f =>
            f.UserId == userId &&
            f.VenueId == dto.VenueId &&
            f.ArtistId == dto.ArtistId &&
            !f.IsDeleted);

        if (existing.Any())
        {
            var ex = existing.First();
            return new FavoriteResponseDto
            {
                Id = ex.Id,
                VenueId = ex.VenueId,
                VenueName = ex.Venue?.Name ?? "",
                ArtistId = ex.ArtistId,
                ArtistName = ex.Artist?.Name ?? ""
            };
        }

        var favorite = new Favorite
        {
            UserId = userId,
            VenueId = dto.VenueId,
            ArtistId = dto.ArtistId
        };

        await _favoriteRepo.AddAsync(favorite);

        return new FavoriteResponseDto
        {
            Id = favorite.Id,
            VenueId = favorite.VenueId,
            VenueName = "",
            ArtistId = favorite.ArtistId,
            ArtistName = ""
        };
    }

    public async Task RemoveAsync(int id, string userId)
    {
        var favorite = await _favoriteRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Favori bulunamadı.");

        if (favorite.UserId != userId)
            throw new UnauthorizedAccessException("Bu favoriyi silme yetkiniz yok.");

        await _favoriteRepo.DeleteAsync(id);
    }
}