using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventTicket.API.Services;

public class AdminService : IAdminService
{
    private readonly IRepository<Event> _eventRepo;
    private readonly IRepository<Venue> _venueRepo;
    private readonly IRepository<Artist> _artistRepo;
    private readonly IRepository<Ticket> _ticketRepo;
    private readonly IRepository<ContactForm> _contactRepo;
    private readonly UserManager<AppUser> _userManager;

    public AdminService(
        IRepository<Event> eventRepo,
        IRepository<Venue> venueRepo,
        IRepository<Artist> artistRepo,
        IRepository<Ticket> ticketRepo,
        IRepository<ContactForm> contactRepo,
        UserManager<AppUser> userManager)
    {
        _eventRepo = eventRepo;
        _venueRepo = venueRepo;
        _artistRepo = artistRepo;
        _ticketRepo = ticketRepo;
        _contactRepo = contactRepo;
        _userManager = userManager;
    }

    public async Task<AdminStatsDto> GetStatsAsync()
    {
        return new AdminStatsDto
        {
            TotalEvents = await _eventRepo.CountAsync(),
            TotalVenues = await _venueRepo.CountAsync(),
            TotalArtists = await _artistRepo.CountAsync(),
            TotalTickets = await _ticketRepo.CountAsync(),
            TotalUsers = await _userManager.Users.CountAsync(),
            PendingEvents = await _eventRepo.CountAsync(e => !e.IsApproved),
            PendingVenues = await _venueRepo.CountAsync(v => !v.IsApproved),
            PendingArtists = await _artistRepo.CountAsync(a => !a.IsApproved)
        };
    }

    public async Task<PendingItemsDto> GetAllPendingAsync()
    {
        var pendingEvents = await _eventRepo.FindWithIncludesAsync(
            e => !e.IsApproved, e => e.Venue, e => e.Artist);
        var pendingVenues = await _venueRepo.FindAsync(v => !v.IsApproved);
        var pendingArtists = await _artistRepo.FindAsync(a => !a.IsApproved);

        return new PendingItemsDto
        {
            Events = pendingEvents.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                VenueId = e.VenueId,
                VenueName = e.Venue?.Name ?? "",
                ArtistId = e.ArtistId,
                ArtistName = e.Artist?.Name ?? "",
                IsApproved = e.IsApproved,
                CreatedAt = e.CreatedAt
            }),
            Venues = pendingVenues.Select(v => new VenueResponseDto
            {
                Id = v.Id,
                Name = v.Name,
                City = v.City,
                IsApproved = v.IsApproved,
                AdminUserId = v.AdminUserId
            }),
            Artists = pendingArtists.Select(a => new ArtistResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Genre = a.Genre,
                IsApproved = a.IsApproved
            })
        };
    }

    public async Task<IEnumerable<ContactFormResponseDto>> GetAllMessagesAsync()
    {
        var messages = await _contactRepo.GetAllAsync();
        return messages
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new ContactFormResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Subject = c.Subject,
                Message = c.Message,
                CreatedAt = c.CreatedAt,
                IsRead = c.IsRead
            });
    }

    public async Task SendMessageAsync(ContactFormDto dto)
    {
        await _contactRepo.AddAsync(new ContactForm
        {
            Name = dto.Name,
            Email = dto.Email,
            Subject = dto.Subject,
            Message = dto.Message,
            IsRead = false
        });
    }

    public async Task MarkAsReadAsync(int id)
    {
        var message = await _contactRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Mesaj bulunamadı.");
        message.IsRead = true;
        await _contactRepo.UpdateAsync(message);
    }

    public async Task DeleteMessageAsync(int id)
        => await _contactRepo.DeleteAsync(id);

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

        var result = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email ?? "",
                Role = roles.FirstOrDefault() ?? "Member",
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            });
        }
        return result;
    }

    public async Task ToggleUserActiveAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        user.IsActive = !user.IsActive;
        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        user.IsActive = false;
        user.Email = $"deleted_{userId}@deleted.com";
        user.UserName = $"deleted_{userId}";
        user.NormalizedEmail = $"DELETED_{userId.ToUpper()}@DELETED.COM";
        user.NormalizedUserName = $"DELETED_{userId.ToUpper()}";
        await _userManager.UpdateAsync(user);
    }
}
