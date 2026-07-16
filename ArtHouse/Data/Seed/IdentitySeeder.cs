using ArtHouse.Identity;
using Microsoft.AspNetCore.Identity;

namespace ArtHouse.Data.Seed
{
    public static class IdentitySeeder
    {

        public static async Task SeedAsync(
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager)
        {

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@arthouse.com");

            if (adminUser == null)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@arthouse.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

        }



    }
}