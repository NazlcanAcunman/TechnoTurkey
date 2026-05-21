using EventTicket.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class HomeController : BaseController
{
    private readonly IEventUlService _eventService;
    private readonly IArtistUlService _artistService;
    private readonly IVenueUlService _venueService;
    private readonly IMessagesUlService _messagesService;
    private readonly IBannerUlService _bannerService;

    public HomeController(IEventUlService eventService, IArtistUlService artistService,
        IVenueUlService venueService, IProfileUlService profileService,
        IMessagesUlService messagesService, IBannerUlService bannerService)
        : base(profileService)
    {
        _eventService = eventService;
        _artistService = artistService;
        _venueService = venueService;
        _messagesService = messagesService;
        _bannerService = bannerService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Anasayfa";
        var events = await _eventService.GetApprovedEventsAsync();
        var artists = await _artistService.GetApprovedArtistsAsync();
        var venues = await _venueService.GetApprovedVenuesAsync();
        var banners = await _bannerService.GetActiveAsync();
        ViewBag.Artists = artists;
        ViewBag.Venues = venues;
        ViewBag.Banners = banners;
        return View(events);
    }

    public IActionResult Privacy() => View();
    public IActionResult Kvkk() { ViewData["Title"] = "KVKK"; return View(); }
    public IActionResult Gizlilik() { ViewData["Title"] = "Gizlilik Politikası"; return View(); }
    public IActionResult Cerez() { ViewData["Title"] = "Çerez Politikası"; return View(); }
    public IActionResult KullanimSartlari() { ViewData["Title"] = "Kullanım Şartları"; return View(); }
    public IActionResult MesafeliSatis() { ViewData["Title"] = "Mesafeli Satış Sözleşmesi"; return View(); }
    public IActionResult OnBilgilendirme() { ViewData["Title"] = "Ön Bilgilendirme Formu"; return View(); }
    public IActionResult AydinlatmaMetni() { ViewData["Title"] = "Aydınlatma Metni"; return View(); }

    public IActionResult Contact()
    {
        ViewData["Title"] = "İletişim";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Contact(string name, string email, string subject, string message)
    {
        ViewData["Title"] = "İletişim";
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
        {
            ViewBag.Error = "Ad, e-posta ve mesaj alanları zorunludur.";
            return View();
        }

        try
        {
            await _messagesService.SendAsync(name, email, subject ?? "", message);
            ViewBag.Success = "Mesajınız iletildi. En kısa sürede size dönüş yapacağız.";
        }
        catch
        {
            ViewBag.Error = "Mesaj gönderilirken bir hata oluştu. Lütfen tekrar deneyin.";
        }
        return View();
    }
}
