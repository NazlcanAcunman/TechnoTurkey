using EventTicket.Api;
using EventTicket.API;
using EventTicket.API.Middleware;
using EventTicket.Data;
using EventTicket.Data.Context;
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

// Rate limiting — giriş/kayıt denemelerine karşı brute-force koruması
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("AuthLimiter", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 8;               // dakikada en fazla 8 deneme (aynı IP)
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
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
