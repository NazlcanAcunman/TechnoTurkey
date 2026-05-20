using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EventTicket.API.Services;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepo;
    private readonly UserManager<AppUser> _userManager;

    public NotificationService(IRepository<Notification> notificationRepo, UserManager<AppUser> userManager)
    {
        _notificationRepo = notificationRepo;
        _userManager = userManager;
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetMyNotificationsAsync(string userId)
    {
        var notifications = await _notificationRepo.FindAsync(n => n.UserId == userId);
        return notifications
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationResponseDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead,
                Type = n.Type,
                CreatedAt = n.CreatedAt
            });
    }

    public async Task<int> GetUnreadCountAsync(string userId)
        => await _notificationRepo.CountAsync(n => n.UserId == userId && !n.IsRead);

    public async Task MarkAsReadAsync(int id, string userId)
    {
        var notification = await _notificationRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Bildirim bulunamadı.");

        if (notification.UserId != userId)
            throw new UnauthorizedAccessException("Bu bildirime erişim yetkiniz yoktur.");

        notification.IsRead = true;
        await _notificationRepo.UpdateAsync(notification);
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        var notifications = await _notificationRepo.FindAsync(
            n => n.UserId == userId && !n.IsRead);

        foreach (var n in notifications)
        {
            n.IsRead = true;
            await _notificationRepo.UpdateAsync(n);
        }
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var notification = await _notificationRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Bildirim bulunamadı.");

        if (notification.UserId != userId)
            throw new UnauthorizedAccessException("Bu bildirime erişim yetkiniz yoktur.");

        await _notificationRepo.DeleteAsync(id);
    }

    public async Task SendToAllAsync(string message, string type)
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            var notification = new Notification
            {
                UserId = user.Id,
                Message = message,
                Type = type,
                IsRead = false
            };
            await _notificationRepo.AddAsync(notification);
        }
    }
}
