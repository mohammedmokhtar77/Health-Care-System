using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence.Repositories;

public class PatientRepository(HealthCareContext context) : IPatientRepository
{

    public async Task<IReadOnlyList<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Patients
                            .AsNoTracking()
                             .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context
            .Patients
            .FirstOrDefaultAsync(p => p.Id == id , cancellationToken);
    }

    public async Task<Patient?> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await context.
            Patients
            .AddAsync(patient, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
        return patient;
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        context
            .Patients
            .Update(patient);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await context.Patients
            .FirstOrDefaultAsync(p => p.Id == id , cancellationToken);
        
        if (patient is null)
            return;

        patient.IsDeleted = true;
        
        await context.SaveChangesAsync(cancellationToken);
    }
}