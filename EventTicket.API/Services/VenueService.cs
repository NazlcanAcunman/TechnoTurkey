using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class VenueService : IVenueService
{
    private readonly IRepository<Venue> _venueRepo;
    private readonly ILogger<VenueService> _logger;

    public VenueService(IRepository<Venue> venueRepo, ILogger<VenueService> logger)
    {
        _venueRepo = venueRepo;
        _logger = logger;
    }

    public async Task<IEnumerable<VenueResponseDto>> GetAllApprovedAsync()
    {
        var venues = await _venueRepo.FindAsync(v => v.IsApproved);
        return venues.Select(MapToDto);
    }

    public async Task<IEnumerable<VenueResponseDto>> GetAllPendingAsync()
    {
        var venues = await _venueRepo.FindAsync(v => !v.IsApproved);
        return venues.Select(MapToDto);
    }

    public async Task<IEnumerable<VenueResponseDto>> GetByAdminAsync(string adminUserId)
    {
        var venues = await _venueRepo.FindAsync(v => v.AdminUserId == adminUserId);
        return venues.Select(MapToDto);
    }

    public async Task<VenueResponseDto?> GetByIdAsync(int id)
    {
        var venue = await _venueRepo.GetByIdAsync(id);
        return venue == null ? null : MapToDto(venue);
    }

    public async Task<VenueResponseDto> CreateAsync(CreateVenueDto dto, string adminUserId)
    {
        var newVenue = new Venue
        {
            Name = dto.Name,
            Description = dto.Description,
            City = dto.City,
            Address = dto.Address,
            IsApproved = true,
            AdminUserId = adminUserId
        };
        await _venueRepo.AddAsync(newVenue);
        _logger.LogInformation("Yeni mekan oluşturuldu: {Name} (ID: {Id})", newVenue.Name, newVenue.Id);
        return MapToDto(newVenue);
    }

    public async Task UpdateAsync(int id, UpdateVenueDto dto, string adminUserId, bool isSuperAdmin)
    {
        var venue = await _venueRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Mekan bulunamadı.");

        if (!isSuperAdmin && venue.AdminUserId != adminUserId)
            throw new UnauthorizedAccessException("Bu mekanı düzenleme yetkiniz bulunmamaktadır.");

        venue.Name = dto.Name;
        venue.Description = dto.Description;
        venue.City = dto.City;
        venue.Address = dto.Address;

        await _venueRepo.UpdateAsync(venue);
        _logger.LogInformation("Mekan güncellendi: ID {Id}", id);
    }

    public async Task DeleteAsync(int id)
    {
        await _venueRepo.DeleteAsync(id);
        _logger.LogInformation("Mekan silindi: ID {Id}", id);
    }

    public async Task ApproveAsync(int id)
    {
        var venue = await _venueRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Mekan bulunamadı.");
        venue.IsApproved = true;
        await _venueRepo.UpdateAsync(venue);
        _logger.LogInformation("Mekan onaylandı: {Name} (ID: {Id})", venue.Name, id);
    }

    public async Task RejectAsync(int id)
    {
        var venue = await _venueRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Mekan bulunamadı.");
        venue.IsApproved = false;
        await _venueRepo.UpdateAsync(venue);
        _logger.LogInformation("Mekan reddedildi: ID {Id}", id);
    }

    private static VenueResponseDto MapToDto(Venue v) => new()
    {
        Id = v.Id,
        Name = v.Name,
        Description = v.Description,
        City = v.City,
        Address = v.Address,
        IsApproved = v.IsApproved,
        AdminUserId = v.AdminUserId
    };
}
