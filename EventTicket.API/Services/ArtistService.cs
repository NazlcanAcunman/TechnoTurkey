using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class ArtistService : IArtistService
{
    private readonly IRepository<Artist> _artistRepo;
    private readonly ILogger<ArtistService> _logger;

    public ArtistService(IRepository<Artist> artistRepo, ILogger<ArtistService> logger)
    {
        _artistRepo = artistRepo;
        _logger = logger;
    }

    public async Task<IEnumerable<ArtistResponseDto>> GetAllApprovedAsync()
    {
        var artists = await _artistRepo.FindAsync(a => a.IsApproved);
        return artists.Select(MapToDto);
    }

    public async Task<IEnumerable<ArtistResponseDto>> GetAllPendingAsync()
    {
        var artists = await _artistRepo.FindAsync(a => !a.IsApproved);
        return artists.Select(MapToDto);
    }

    public async Task<ArtistResponseDto?> GetByIdAsync(int id)
    {
        var artist = await _artistRepo.GetByIdAsync(id);
        return artist == null ? null : MapToDto(artist);
    }

    public async Task<ArtistResponseDto> CreateAsync(CreateArtistDto dto)
    {
        var newArtist = new Artist
        {
            Name = dto.Name,
            Bio = dto.Bio,
            Genre = dto.Genre,
            ImageUrl = dto.ImageUrl,
            IsApproved = true
        };
        await _artistRepo.AddAsync(newArtist);
        _logger.LogInformation("Yeni sanatçı oluşturuldu: {Name}", newArtist.Name);
        return MapToDto(newArtist);
    }

    public async Task UpdateAsync(int id, UpdateArtistDto dto)
    {
        var existing = await _artistRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Sanatçı bulunamadı.");

        existing.Name = dto.Name;
        existing.Bio = dto.Bio;
        existing.Genre = dto.Genre;
        existing.ImageUrl = dto.ImageUrl;

        await _artistRepo.UpdateAsync(existing);
        _logger.LogInformation("Sanatçı güncellendi: ID {Id}", id);
    }

    public async Task DeleteAsync(int id)
    {
        await _artistRepo.DeleteAsync(id);
        _logger.LogInformation("Sanatçı silindi: ID {Id}", id);
    }

    public async Task ApproveAsync(int id)
    {
        var existing = await _artistRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Sanatçı bulunamadı.");
        existing.IsApproved = true;
        await _artistRepo.UpdateAsync(existing);
        _logger.LogInformation("Sanatçı onaylandı: ID {Id}", id);
    }

    public async Task RejectAsync(int id)
    {
        var existing = await _artistRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Sanatçı bulunamadı.");
        existing.IsApproved = false;
        await _artistRepo.UpdateAsync(existing);
        _logger.LogInformation("Sanatçı reddedildi: ID {Id}", id);
    }

    private static ArtistResponseDto MapToDto(Artist a) => new()
    {
        Id = a.Id,
        Name = a.Name,
        Bio = a.Bio,
        Genre = a.Genre,
        ImageUrl = a.ImageUrl,
        IsApproved = a.IsApproved
    };
}