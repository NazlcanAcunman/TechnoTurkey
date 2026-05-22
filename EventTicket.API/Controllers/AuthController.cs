using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Enums;
using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (dto.DateOfBirth > DateTime.Today.AddYears(-18))
            return BadRequest("Kayıt olabilmek için 18 yaşından büyük olmanız gerekmektedir.");

        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            return BadRequest("Bu e-posta adresi zaten kayıtlı.");

        // Boşlukları kaldır — Identity UserName'de boşluğa izin vermiyor
        var safeUserName = dto.UserName.Replace(" ", "").Replace(".", "").ToLower();
        if (safeUserName.Length < 2) safeUserName = "user" + safeUserName;

        // UserName benzersiz olmalı; çakışırsa email öneki kullan
        var existingByName = await _userManager.FindByNameAsync(safeUserName);
        if (existingByName != null)
            safeUserName = dto.Email.Split('@')[0] + new Random().Next(100, 999);

        var dob = dto.DateOfBirth.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc)
            : dto.DateOfBirth.ToUniversalTime();

        var user = new AppUser
        {
            UserName       = safeUserName,
            FullName       = dto.FullName,
            Email          = dto.Email,
            TcKimlikNo     = dto.TcKimlikNo,
            DateOfBirth    = dob,
            IsActive       = true,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(user, Roles.Member);

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);
        var expireDays = int.TryParse(_configuration["Jwt:ExpireDays"], out var d) ? d : 7;

        return Ok(new AuthResponseDto
        {
            UserId = user.Id,       
            Token = token,
            UserName = user.UserName,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? "",
            IsActive = user.IsActive,
            ExpiresAt = DateTime.UtcNow.AddDays(expireDays)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("E-posta veya şifre hatalı.");

        if (!user.IsActive)
            return Unauthorized("Bu hesap devre dışı bırakılmıştır.");

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
            return Unauthorized("Çok fazla başarısız deneme. Lütfen 5 dakika sonra tekrar deneyin.");

        if (result.IsNotAllowed)
            return Unauthorized("Bu hesap henüz doğrulanmamış.");

        if (!result.Succeeded)
            return Unauthorized("E-posta veya şifre hatalı.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);
        var expireDays = int.TryParse(_configuration["Jwt:ExpireDays"], out var d) ? d : 7;

        return Ok(new AuthResponseDto
        {
            UserId = user.Id,     
            Token = token,
            UserName = user.UserName,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? "",
            IsActive = user.IsActive, 
            ExpiresAt = DateTime.UtcNow.AddDays(expireDays)
        });
    }

    [Authorize]
    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Kullanıcı bulunamadı.");

        user.IsActive = false;
        user.Email = $"deleted_{userId}@deleted.com";
        user.UserName = $"deleted_{userId}";
        user.NormalizedEmail = $"DELETED_{userId.ToUpper()}@DELETED.COM";
        user.NormalizedUserName = $"DELETED_{userId.ToUpper()}";

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return BadRequest("Hesap silinirken hata oluştu.");

        return Ok(new { message = "Hesabınız başarıyla silindi." });
    }
}
