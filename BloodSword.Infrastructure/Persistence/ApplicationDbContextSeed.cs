using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BloodSword.Application.Contracts;
using System.Threading.Tasks;

namespace BloodSword.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Създаване на ролите
            await EnsureRoleExistsAsync(roleManager, UserRoles.Admin);
            await EnsureRoleExistsAsync(roleManager, UserRoles.Player);

            // 2. Създаване на първия Admin потребител
            const string adminUsername = "admin";
            const string adminPassword = "SecureP@ssw0rd1!"; // МОЛЯ, СМЕНИ ТАЗИ ПАРОЛА!

            var adminUser = await userManager.FindByNameAsync(adminUsername);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminUsername,
                    Email = "admin@bloodsword.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // 3. Присвояване на Admin ролята
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }
            }
        }

        private static async Task EnsureRoleExistsAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}