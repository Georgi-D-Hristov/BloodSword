using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BloodSword.Application.Contracts;

namespace BloodSword.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureRoleExistsAsync(roleManager, UserRoles.Admin);
            await EnsureRoleExistsAsync(roleManager, UserRoles.Player);

            const string adminUsername = "admin";
            const string adminPassword = "SecureP@ssw0rd1!";

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