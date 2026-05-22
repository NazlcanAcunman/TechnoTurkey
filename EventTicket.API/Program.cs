using EventTicket.Api;
using EventTicket.API;
using EventTicket.API.Middleware;
using EventTicket.Data;
using EventTicket.Data.Context;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Veri katmanı (DB + Repository + UnitOfWork)
builder.Services.AddDataServices(builder.Configuration);

// API servisleri (Identity + JWT + Services + AutoMapper + FluentValidation)
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddControllers();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await DbSeeder.SeedData(app);

app.Run();

