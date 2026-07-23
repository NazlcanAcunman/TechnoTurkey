using EventTicket.Core.Entities;
using EventTicket.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AcademyController : ControllerBase
{
    private readonly AppDbContext _db;

    public AcademyController(AppDbContext db)
    {
        _db = db;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _db.AcademyContents
            .Where(x => !x.IsDeleted && x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .Select(x => new
            {
                x.Id,
                x.Type,
                x.Title,
                x.Description,
                x.MediaUrl,
                x.LinkUrl,
                x.DisplayOrder,
                x.IsActive
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Create([FromBody] AcademyContentDto dto)
    {
        var item = new AcademyContent
        {
            Type = dto.Type,
            Title = dto.Title,
            Description = dto.Description,
            MediaUrl = dto.MediaUrl,
            LinkUrl = dto.LinkUrl,
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        _db.AcademyContents.Add(item);
        await _db.SaveChangesAsync();

        return Ok(new { item.Id, item.Type, item.Title, item.Description, item.MediaUrl, item.LinkUrl, item.DisplayOrder, item.IsActive });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] AcademyContentDto dto)
    {
        var item = await _db.AcademyContents.FindAsync(id);
        if (item == null || item.IsDeleted) return NotFound();

        item.Type = dto.Type;
        item.Title = dto.Title;
        item.Description = dto.Description;
        item.MediaUrl = dto.MediaUrl;
        item.LinkUrl = dto.LinkUrl;
        item.DisplayOrder = dto.DisplayOrder;
        item.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.AcademyContents.FindAsync(id);
        if (item == null || item.IsDeleted) return NotFound();

        item.IsDeleted = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public class AcademyContentDto
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MediaUrl { get; set; }
    public string? LinkUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
