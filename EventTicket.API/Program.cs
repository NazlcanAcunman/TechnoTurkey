using EventTicket.Api;
using EventTicket.API;
using EventTicket.API.Middleware;
using EventTicket.Data;
using EventTicket.Data.Context;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

// Npgsql: treat all DateTime values as UTC (fixes DateTimeKind.Unspecified → timestamp with time zone)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Veri katmanı (DB + Repository + UnitOfWork)
builder.Services.AddDataServices(builder.Configuration);

// API servisleri (Identity + JWT + Services + AutoMapper + FluentValidation)
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthLimiter", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 30,
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));
});
builder.Services.AddOpenApi();

var app = builder.Build();

// Veritabanı migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "EventTicket API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseForwardedHeaders();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowUI");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await DbSeeder.SeedData(app);

app.Run();
