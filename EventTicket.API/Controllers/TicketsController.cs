using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "MemberOrAbove")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly UserManager<AppUser> _userManager;

    public TicketsController(ITicketService ticketService, UserManager<AppUser> userManager)
    {
        _ticketService = ticketService;
        _userManager = userManager;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase([FromBody] PurchaseTicketDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Unauthorized();

        if (!user.IsActive)
            return Unauthorized("Bu hesap devre dışı bırakılmıştır.");

        if (string.IsNullOrWhiteSpace(user.TcKimlikNo))
            return BadRequest("Bilet alabilmek için profilinizde TC Kimlik No girmeniz gerekiyor.");

        if (user.DateOfBirth.HasValue && user.DateOfBirth.Value > DateTime.Today.AddYears(-18))
            return BadRequest("Bilet alabilmek için 18 yaşından büyük olmanız gerekiyor.");

        var tickets = await _ticketService.PurchaseAsync(dto, userId);
        return StatusCode(201, tickets);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Ok(await _ticketService.GetMyTicketsAsync(userId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin") || User.IsInRole("SuperAdmin");
        if (!isAdmin && ticket.UserId != userId)
            return Forbid();

        return Ok(ticket);
    }
}
