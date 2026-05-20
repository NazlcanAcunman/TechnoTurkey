using EventTicket.Core.DTOs;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IAdminService _adminService;

    public ContactController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ContactFormDto dto)
    {
        await _adminService.SendMessageAsync(dto);
        return Ok(new { message = "Mesajınız alındı. En kısa sürede size dönüş yapacağız." });
    }
}
