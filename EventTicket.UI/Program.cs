using EventTicket.Core.Interfaces;
using EventTicket.UI.Services;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.RateLimiting;

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
        options.Cookie.MaxAge = TimeSpan.FromDays(7);
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();

// ApiService — API'ye istek atmak için
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    // Self-signed sertifikaya SADECE Development'ta izin ver (production'da normal doğrulama)
    if (builder.Environment.IsDevelopment())
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
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

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, token) =>
    {
        ctx.HttpContext.Response.ContentType = "text/plain; charset=utf-8";
        await ctx.HttpContext.Response.WriteAsync("Çok fazla deneme yaptınız. Lütfen bir dakika sonra tekrar deneyin.", token);
    };
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
    {
        var path = ctx.Request.Path.Value ?? "";
        var isAuthPost = HttpMethods.IsPost(ctx.Request.Method) &&
            (path.StartsWith("/Auth/Login", StringComparison.OrdinalIgnoreCase) ||
             path.StartsWith("/Auth/Register", StringComparison.OrdinalIgnoreCase));

        if (!isAuthPost)
            return RateLimitPartition.GetNoLimiter("other");

        var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter("auth:" + ip, _ => new FixedWindowRateLimiterOptions
        {
            Window = TimeSpan.FromMinutes(1),
            PermitLimit = 8,
            QueueLimit = 0
        });
    });
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
