using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/artists")]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _artistService.GetAllApprovedAsync());

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        return artist == null ? NotFound() : Ok(artist);
    }

    [HttpGet("pending")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> GetPending()
        => Ok(await _artistService.GetAllPendingAsync());

    [HttpPost]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Create([FromBody] CreateArtistDto dto)
    {
        var created = await _artistService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArtistDto dto)
    {
        await _artistService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        await _artistService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Approve(int id)
    {
        await _artistService.ApproveAsync(id);
        return Ok(new { message = "Sanatçı onaylandı." });
    }

    [HttpPatch("{id}/reject")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Reject(int id)
    {
        await _artistService.RejectAsync(id);
        return Ok(new { message = "Sanatçı reddedildi." });
    }
}
