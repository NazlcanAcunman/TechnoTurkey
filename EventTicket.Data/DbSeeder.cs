using EventTicket.Core.Entities;
using EventTicket.Core.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventTicket.Api;

public class DbSeeder
{
    public static async Task SeedData(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        try
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var hasher      = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();
            var config      = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            foreach (var role in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new AppRole(role));
            }
            
            var adminEmail = config["Seed:AdminEmail"];
            if (string.IsNullOrWhiteSpace(adminEmail))
                adminEmail = "technoturkey@gmail.com";

            var adminPassword = config["Seed:AdminPassword"];
            var forceReset = string.Equals(config["Seed:ForcePasswordReset"], "true", StringComparison.OrdinalIgnoreCase);

            var superAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (superAdmin == null)
            {
                if (string.IsNullOrWhiteSpace(adminPassword))
                {
                    Console.WriteLine("SuperAdmin yok ve Seed__AdminPassword tanımlı değil; süper admin oluşturulmadı.");
                    return;
                }

                superAdmin = new AppUser
                {
                    UserName       = "superadmin",
                    Email          = adminEmail,
                    EmailConfirmed = true,
                    SecurityStamp  = Guid.NewGuid().ToString(),
                    IsActive       = true,
                };
                var createResult = await userManager.CreateAsync(superAdmin, adminPassword);
                if (!createResult.Succeeded)
                {
                    Console.WriteLine("SuperAdmin oluşturulamadı: " +
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    return;
                }
                Console.WriteLine("SuperAdmin oluşturuldu.");
            }
            else
            {
                superAdmin.IsActive       = true;
                superAdmin.EmailConfirmed = true;
                superAdmin.LockoutEnd     = null;

                if (forceReset && !string.IsNullOrWhiteSpace(adminPassword))
                {
                    superAdmin.PasswordHash  = hasher.HashPassword(superAdmin, adminPassword);
                    superAdmin.SecurityStamp = Guid.NewGuid().ToString();
                    Console.WriteLine("SuperAdmin şifresi Seed__AdminPassword ile sıfırlandı. " +
                                      "Seed__ForcePasswordReset değişkenini şimdi silin.");
                }

                var updateResult = await userManager.UpdateAsync(superAdmin);
                if (!updateResult.Succeeded)
                    Console.WriteLine("SuperAdmin güncellenemedi: " +
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)));

                await userManager.ResetAccessFailedCountAsync(superAdmin);
            }

            // SuperAdmin rolünü garantiye al
            var currentRoles = await userManager.GetRolesAsync(superAdmin);
            if (!currentRoles.Contains(Roles.SuperAdmin))
            {
                await userManager.RemoveFromRolesAsync(superAdmin, currentRoles);
                await userManager.AddToRoleAsync(superAdmin, Roles.SuperAdmin);
                Console.WriteLine("SuperAdmin rolü atandı.");
            }
            else
            {
                Console.WriteLine("SuperAdmin rolü zaten mevcut.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Seeder hatası: " + ex.Message);
        }
    }
}
