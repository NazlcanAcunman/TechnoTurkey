using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannersController : ControllerBase
{
    private readonly IBannerService _svc;

    public BannersController(IBannerService svc) => _svc = svc;

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetActive() => Ok(await _svc.GetActiveAsync());

    [HttpGet("all")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> GetById(int id)
    {
        var b = await _svc.GetByIdAsync(id);
        return b == null ? NotFound() : Ok(b);
    }

    [HttpPost]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateBannerDto dto)
    {
        var created = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBannerDto dto)
    {
        await _svc.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}
