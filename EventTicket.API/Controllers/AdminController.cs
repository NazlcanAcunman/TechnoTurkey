using EventTicket.Core.Entities;
using EventTicket.Core.Enums;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "SuperAdminOnly")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly UserManager<AppUser> _userManager;

    public AdminController(IAdminService adminService, UserManager<AppUser> userManager)
    {
        _adminService = adminService;
        _userManager = userManager;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
        => Ok(await _adminService.GetStatsAsync());

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
        => Ok(await _adminService.GetAllPendingAsync());

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
        => Ok(await _adminService.GetAllUsersAsync());

    [HttpPatch("users/{userId}/toggle-active")]
    public async Task<IActionResult> ToggleUserActive(string userId)
    {
        await _adminService.ToggleUserActiveAsync(userId);
        return Ok(new { message = "Kullanıcı durumu güncellendi." });
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _adminService.DeleteUserAsync(userId);
        return Ok(new { message = "Kullanıcı silindi." });
    }

    [HttpPatch("users/{userId}/assign-admin")]
    public async Task<IActionResult> AssignAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Kullanıcı bulunamadı.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, Roles.Admin);
        return Ok(new { message = "Kullanıcı Admin olarak atandı." });
    }

    [HttpPatch("users/{userId}/assign-member")]
    public async Task<IActionResult> AssignMember(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Kullanıcı bulunamadı.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, Roles.Member);
        return Ok(new { message = "Kullanıcı Üye olarak atandı." });
    }

    [HttpGet("contact")]
    public async Task<IActionResult> GetAllMessages()
        => Ok(await _adminService.GetAllMessagesAsync());

    [HttpPatch("contact/{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _adminService.MarkAsReadAsync(id);
        return Ok(new { message = "Okundu olarak işaretlendi." });
    }

    [HttpDelete("contact/{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        await _adminService.DeleteMessageAsync(id);
        return Ok(new { message = "Mesaj silindi." });
    }
}