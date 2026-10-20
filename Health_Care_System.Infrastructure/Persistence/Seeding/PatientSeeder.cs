using System.Text.Json;
using Health_Care_System.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence.Seeding;

public static class PatientSeeder
{
    public static async Task SeedAsync(
        HealthCareContext context,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        if (await context.Patients
                .IgnoreQueryFilters()
                .AnyAsync(cancellationToken))
        {
            return;
        }

        await using var stream = File.OpenRead(filePath);

        var patients = await JsonSerializer.DeserializeAsync<List<Patient>>(
            stream,
            cancellationToken: cancellationToken);

        if (patients is null || patients.Count == 0)
        {
            return;
        }

        context.Patients.AddRange(patients);

        await context.SaveChangesAsync(cancellationToken);
    }
}