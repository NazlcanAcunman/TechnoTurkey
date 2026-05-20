using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [AllowAnonymous]
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetByEvent(int eventId)
        => Ok(await _commentService.GetByEventAsync(eventId));

    [AllowAnonymous]
    [HttpGet("venue/{venueId}")]
    public async Task<IActionResult> GetByVenue(int venueId)
        => Ok(await _commentService.GetByVenueAsync(venueId));

    [Authorize(Policy = "MemberOrAbove")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCommentDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
       
        if (userId == null) return Unauthorized();

        var userFullName = User.FindFirstValue(ClaimTypes.Name) ?? "Anonim";

        var result = await _commentService.AddAsync(dto, userId, userFullName);
        return StatusCode(201, result);
    }

    [Authorize(Policy = "SuperAdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _commentService.DeleteAsync(id);
        return NoContent();
    }
}