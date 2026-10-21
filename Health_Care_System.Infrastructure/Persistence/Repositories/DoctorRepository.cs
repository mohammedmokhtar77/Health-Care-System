using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence.Repositories;

public class DoctorRepository(HealthCareContext context) : IDoctorRepository
{
    public async Task<IReadOnlyList<Doctor>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Doctors
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await context.Doctors
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Doctor> AddAsync(
        Doctor doctor,
        CancellationToken cancellationToken = default)
    {
        await context.Doctors.AddAsync(doctor, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return doctor;
    }

    public async Task UpdateAsync(
        Doctor doctor,
        CancellationToken cancellationToken = default)
    {
        context.Doctors.Update(doctor);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var doctor = await context.Doctors
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (doctor is null)
            return;
        doctor.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
    }
}