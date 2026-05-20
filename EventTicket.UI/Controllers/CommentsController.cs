using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class CommentsController : BaseController
{
    private readonly ICommentUlService _commentService;

    public CommentsController(ICommentUlService commentService, IProfileUlService profileService)
        : base(profileService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, int? returnEventId, int? returnVenueId)
    {
        await _commentService.DeleteAsync(id);

        if (returnEventId.HasValue)
            return RedirectToAction("Details", "Events", new { id = returnEventId });

        if (returnVenueId.HasValue)
            return RedirectToAction("Details", "Venues", new { id = returnVenueId });

        return RedirectToAction("Index", "Home");
    }
}