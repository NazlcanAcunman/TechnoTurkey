using EventTicket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;

    public FileController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost("upload")]
    [Authorize(Policy = "AdminOrAbove")]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string folder = "events")
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Lütfen bir dosya seçin." });

        var allowedFolders = new[] { "events", "venues", "artists" };
        if (!allowedFolders.Contains(folder))
            return BadRequest(new { message = "Geçersiz klasör. İzin verilenler: events, venues, artists." });

        const long maxBytes = 5 * 1024 * 1024;
        if (file.Length > maxBytes)
            return BadRequest(new { message = "Dosya boyutu 5 MB'ı aşamaz." });

        using var stream = file.OpenReadStream();
        var result = await _fileUploadService.UploadAsync(stream, file.FileName, folder);
        return Ok(new { filePath = result });
    }

    [HttpGet("list")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> List([FromQuery] string folder = "events")
    {
        var files = await _fileUploadService.ListAsync(folder);
        return Ok(files);
    }

    [HttpDelete]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> Delete([FromQuery] string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || relativePath.Contains(".."))
            return BadRequest(new { message = "Geçersiz dosya yolu." });

        var deleted = await _fileUploadService.DeleteAsync(relativePath);
        return deleted ? Ok(new { message = "Dosya silindi." }) : NotFound(new { message = "Dosya bulunamadı." });
    }
}