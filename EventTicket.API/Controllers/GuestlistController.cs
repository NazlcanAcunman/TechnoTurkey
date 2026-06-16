using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestlistController : ControllerBase
{
    private readonly IGuestlistService _guestlistService;

    public GuestlistController(IGuestlistService guestlistService)
    {
        _guestlistService = guestlistService;
    }

    [Authorize(Policy = "MemberOrAbove")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestlistRequestDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _guestlistService.CreateAsync(dto, userId);
        if (!result.Success) return BadRequest(new { message = result.Message });

        return StatusCode(201, result.Data);
    }

    [Authorize(Policy = "MemberOrAbove")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _guestlistService.GetByUserAsync(userId);
        return Ok(result.Data);
    }

    [Authorize(Policy = "AdminOrAbove")]
    [HttpGet("event/{eventId:int}")]
    public async Task<IActionResult> GetByEvent(int eventId)
    {
        var result = await _guestlistService.GetByEventAsync(eventId);
        return Ok(result.Data);
    }

    [Authorize(Policy = "AdminOrAbove")]
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateGuestlistStatusDto dto)
    {
        var result = await _guestlistService.UpdateStatusAsync(id, dto);
        if (!result.Success) return NotFound(new { message = result.Message });

        return Ok(result.Data);
    }

    [Authorize(Policy = "AdminOrAbove")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _guestlistService.DeleteAsync(id);
        if (!result.Success) return NotFound(new { message = result.Message });

        return NoContent();
    }

    [Authorize(Policy = "AdminOrAbove")]
    [HttpGet("event/{eventId:int}/stats")]
    public async Task<IActionResult> GetStats(int eventId)
    {
        var result = await _guestlistService.GetStatsAsync(eventId);
        return Ok(result.Data);
    }
}
