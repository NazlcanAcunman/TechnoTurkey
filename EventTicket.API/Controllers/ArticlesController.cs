using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _svc;

    public ArticlesController(IArticleService svc) => _svc = svc;

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetPublished() => Ok(await _svc.GetPublishedAsync());

    [HttpGet("all")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var a = await _svc.GetByIdAsync(id);
        return a == null ? NotFound() : Ok(a);
    }

    [AllowAnonymous]
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var a = await _svc.GetBySlugAsync(slug);
        return a == null ? NotFound() : Ok(a);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Create([FromBody] CreateArticleDto dto)
    {
        var created = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleDto dto)
    {
        await _svc.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}
