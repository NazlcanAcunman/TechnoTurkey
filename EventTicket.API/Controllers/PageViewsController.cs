using EventTicket.Core.Entities;
using EventTicket.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PageViewsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PageViewsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Track()
    {
        _db.PageViews.Add(new PageView { ViewedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();
        return Ok();
    }
}
