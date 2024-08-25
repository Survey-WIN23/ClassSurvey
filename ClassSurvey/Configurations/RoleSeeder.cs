using ClassSurvey.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClassSurvey.Configurations;

public static class RoleSeeder
{
    public static async Task SeedSuperAdminAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

        string[] roles = ["SuperUser"];
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

        string adminEmail = "admin@example.com";
        string adminPassword = "Admin123!";

        if (userManager.Users.All(u => u.UserName != "ted.pieplow@gmail.com"))
        {
            var adminUser = new UserEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "SuperUser");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
        }
    }
}
