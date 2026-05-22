using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _eventService.GetAllApprovedAsync());

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evt = await _eventService.GetByIdAsync(id);
        return evt == null ? NotFound() : Ok(evt);
    }

    [AllowAnonymous]
    [HttpGet("venue/{venueId}")]
    public async Task<IActionResult> GetByVenue(int venueId)
        => Ok(await _eventService.GetByVenueAsync(venueId));

    [HttpGet("all")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> GetAll_Admin()
    {
        var all = await _eventService.GetAllForAdminAsync();
        return Ok(all.OrderByDescending(e => e.Date));
    }

    [HttpGet("pending")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> GetPending()
        => Ok(await _eventService.GetAllPendingAsync());

    [HttpPost]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isSuperAdmin = User.IsInRole("SuperAdmin");
        var created = await _eventService.CreateAsync(dto, userId, isSuperAdmin);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isSuperAdmin = User.IsInRole("SuperAdmin");
        await _eventService.UpdateAsync(id, dto, userId, isSuperAdmin);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        await _eventService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Approve(int id)
    {
        await _eventService.ApproveAsync(id);
        return Ok(new { message = "Etkinlik onaylandı." });
    }

    [HttpPatch("{id}/reject")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Reject(int id, [FromBody] RejectEventDto dto)
    {
        await _eventService.RejectAsync(id, dto.Reason);
        return Ok(new { message = "Etkinlik reddedildi." });
    }
}

