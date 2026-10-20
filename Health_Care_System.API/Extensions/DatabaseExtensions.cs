using Health_Care_System.Infrastructure.Persistence;
using Health_Care_System.Infrastructure.Persistence.Seeding;
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

            logger.LogInformation("Database migration and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}