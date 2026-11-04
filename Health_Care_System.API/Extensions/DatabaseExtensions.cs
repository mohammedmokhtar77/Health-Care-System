using Health_Care_System.Infrastructure.Persistence;
using Health_Care_System.Infrastructure.Persistence.Seeding;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.API.Extensions;

public static class DatabaseExtensions
{
    public static async Task MigrateAndSeedDatabaseAsync(
        this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var services = scope.ServiceProvider;
        var logger = services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseInitialization");

        try
        {
            var context = services.GetRequiredService<HealthCareContext>();

            // Apply pending migrations.
            await context.Database.MigrateAsync();

            var dataDirectory = Path.Combine(
                AppContext.BaseDirectory, "Persistence", "Seeding", "Data");

            // Seed Doctors and Patients.
            await DoctorSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Doctors.json"));

            await PatientSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Patients.json"));
            
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await RolesSeeder.SeedRolesAsync(roleManager);

            await SeedBootstrapAdminAsync(
                services.GetRequiredService<UserManager<ApplicationUser>>(),
                app.Configuration,
                logger);
            
            logger.LogInformation("Database migration and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    private static async Task SeedBootstrapAdminAsync(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ILogger logger)
    {
        var email = configuration["BootstrapAdmin:Email"];
        var password = configuration["BootstrapAdmin:Password"];
        var fullName = configuration["BootstrapAdmin:FullName"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogInformation("Bootstrap Admin credentials are not configured; skipping initial Admin creation.");
            return;
        }

        if (await userManager.FindByEmailAsync(email) is not null)
        {
            logger.LogInformation("Bootstrap Admin email already exists; no account or role changes were made.");
            return;
        }

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = string.IsNullOrWhiteSpace(fullName) ? "System Admin" : fullName
        };

        var createResult = await userManager.CreateAsync(admin, password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Could not create bootstrap Admin: {errors}");
        }

        var roleResult = await userManager.AddToRoleAsync(admin, Roles.Admin);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(admin);
            var errors = string.Join(", ", roleResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Could not assign the Admin role to bootstrap account: {errors}");
        }

        logger.LogInformation("Bootstrap Admin account created successfully.");
    }
}
