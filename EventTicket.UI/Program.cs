using EventTicket.Core.Interfaces;
using EventTicket.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Cookie Auth — UI tarafı için
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        // MaxAge makes the browser persist the cookie across restarts
        // (ExpireTimeSpan alone is not enough without IsPersistent on each SignIn,
        //  but MaxAge ensures the browser honours the lifetime even when set).
        options.Cookie.MaxAge = TimeSpan.FromDays(7);
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

// ApiService — API'ye istek atmak için
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Development'ta self-signed sertifikaya izin ver
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

builder.Services.AddScoped<IEventUlService, EventUlService>();
builder.Services.AddScoped<IAdminUlService, AdminUlService>();
builder.Services.AddScoped<IAuthUlService, AuthUlService>();
builder.Services.AddScoped<IVenueUlService, VenueUlService>();
builder.Services.AddScoped<IArtistUlService, ArtistUlService>();
builder.Services.AddScoped<ICommentUlService, CommentUlService>();
builder.Services.AddScoped<ITicketUlService, TicketUlService>();
builder.Services.AddScoped<IMessagesUlService, MessagesUlService>();
builder.Services.AddScoped<IProfileUlService, ProfileUlService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAcademyUlService, AcademyUlService>();
builder.Services.AddScoped<INotificationUlService, NotificationUlService>();
builder.Services.AddScoped<IBannerUlService, BannerUlService>();
builder.Services.AddScoped<IArticleUlService, ArticleUlService>();
builder.Services.AddScoped<IGuestlistUiService, GuestlistUiService>();


// Session — sepet için
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Site ziyaret sayacı — her sayfa yüklenişinde API'ye bildir (statik, admin ve api hariç)
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    var isTracked = context.Request.Method == "GET"
        && !path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase)
        && !path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
        && !path.StartsWith("/css", StringComparison.OrdinalIgnoreCase)
        && !path.StartsWith("/js", StringComparison.OrdinalIgnoreCase)
        && !path.StartsWith("/lib", StringComparison.OrdinalIgnoreCase)
        && !path.StartsWith("/images", StringComparison.OrdinalIgnoreCase)
        && !path.Contains(".", StringComparison.Ordinal);

    if (isTracked)
    {
        var http = context.RequestServices.GetRequiredService<IHttpClientFactory>();
        var config = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiBase = config["ApiSettings:BaseUrl"]!;
        _ = Task.Run(async () =>
        {
            try
            {
                using var client = http.CreateClient();
                await client.PostAsync($"{apiBase}api/pageviews", null);
            }
            catch { /* sayaç hatası sitenin çalışmasını engellemesin */ }
        });
    }

    await next();
});

// Area route — üstte olmalı
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Normal route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
