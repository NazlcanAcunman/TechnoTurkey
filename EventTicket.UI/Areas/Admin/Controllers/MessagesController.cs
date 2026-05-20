using EventTicket.UI.Services;
using EventTicket.UI.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class MessagesController : Controller
{
    private readonly IMessagesUlService _messagesService;

    public MessagesController(IMessagesUlService messagesService)
    {
        _messagesService = messagesService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Mesajlar";
        var messages = await _messagesService.GetAllAsync();
        return View(messages);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _messagesService.MarkAsReadAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _messagesService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}