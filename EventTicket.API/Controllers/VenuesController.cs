using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    private readonly IVenueService _venueService;

    public VenuesController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _venueService.GetAllApprovedAsync());

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var venue = await _venueService.GetByIdAsync(id);
        return venue == null ? NotFound() : Ok(venue);
    }

    [HttpGet("pending")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> GetPending()
        => Ok(await _venueService.GetAllPendingAsync());

    [HttpGet("my")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> GetMyVenues()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Ok(await _venueService.GetByAdminAsync(userId));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Create([FromBody] CreateVenueDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var created = await _venueService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVenueDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isSuperAdmin = User.IsInRole("SuperAdmin");
        await _venueService.UpdateAsync(id, dto, userId, isSuperAdmin);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        await _venueService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Approve(int id)
    {
        await _venueService.ApproveAsync(id);
        return Ok(new { message = "Mekan onaylandı." });
    }

    [HttpPatch("{id}/reject")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Reject(int id)
    {
        await _venueService.RejectAsync(id);
        return Ok(new { message = "Mekan reddedildi." });
    }
}
