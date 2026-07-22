using EventTicket.Core.Entities;
using EventTicket.Core.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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

            // Tüm rolleri oluştur
            foreach (var role in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new AppRole(role));
            }

            const string adminEmail    = "technoturkey@gmail.com";
            const string adminPassword = "Admin.123";

            var superAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (superAdmin == null)
            {
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
             
                superAdmin.PasswordHash   = hasher.HashPassword(superAdmin, adminPassword);
                superAdmin.SecurityStamp  = Guid.NewGuid().ToString();
                superAdmin.IsActive       = true;
                superAdmin.EmailConfirmed = true;
                superAdmin.LockoutEnd     = null;

                var updateResult = await userManager.UpdateAsync(superAdmin);
                if (!updateResult.Succeeded)
                    Console.WriteLine("SuperAdmin güncellenemedi: " +
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                else
                    Console.WriteLine("SuperAdmin şifresi sıfırlandı (Admin.123).");

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
