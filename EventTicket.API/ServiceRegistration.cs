using EventTicket.API.Services;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;
using EventTicket.Core.Validators;
using EventTicket.Data.Context;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventTicket.API;

public static class ServiceRegistration
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentityCore<AppUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<AppRole>()
        .AddSignInManager()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<TurkishIdentityErrorDescriber>();

        var jwtKey = configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new InvalidOperationException("Jwt:Key (veya Jwt__Key env var) boş ya da tanımlı değil.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("SuperAdminOnly", p => p.RequireRole("SuperAdmin"));
            options.AddPolicy("AdminOrAbove", p => p.RequireRole("Admin", "SuperAdmin"));
            options.AddPolicy("MemberOrAbove", p => p.RequireRole("Member", "Admin", "SuperAdmin"));
        });

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IVenueService, VenueService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IGuestlistService, GuestlistService>();

        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

        services.AddFluentValidationAutoValidation();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowUI", policy =>
                policy.WithOrigins(
                    "https://localhost:7052",
                    "http://localhost:5161",
                    "https://technoturkey.onrender.com",
                    "https://technoturkey.net",
                    "https://www.technoturkey.net"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
        });

        return services;
    }
}