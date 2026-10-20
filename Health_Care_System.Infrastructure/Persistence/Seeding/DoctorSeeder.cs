using System.Text.Json;
using Health_Care_System.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence.Seeding;

public static class DoctorSeeder
{
    public static async Task SeedAsync(
        HealthCareContext context,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        if (await context.Doctors
                .IgnoreQueryFilters()
                .AnyAsync(cancellationToken))
        {
            return;
        }

        await using var stream = File.OpenRead(filePath);

        var doctors = await JsonSerializer.DeserializeAsync<List<Doctor>>(
            stream,
            cancellationToken: cancellationToken);

        if (doctors is null || doctors.Count == 0)
        {
            return;
        }

        context.Doctors.AddRange(doctors);

        await context.SaveChangesAsync(cancellationToken);
    }
}