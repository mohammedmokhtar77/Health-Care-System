
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence.Repositories;

public class AppointmentRepository(HealthCareContext context)
    : IAppointmentRepository
{
    public async Task<IReadOnlyList<Appointment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Appointments
            .AsNoTracking()
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken = default)
    {
        return await context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(
                a => a.PatientId == patientId &&
                     a.DoctorId == doctorId,
                cancellationToken);
    }

    public async Task<Appointment> AddAsync(
        Appointment appointment,
        CancellationToken cancellationToken = default)
    {
        await context.Appointments.AddAsync(appointment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return appointment;
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        var existingAppointment = await context.Appointments
            .FirstOrDefaultAsync(a => a.PatientId == appointment.PatientId && a.DoctorId == appointment.DoctorId, cancellationToken);

        if (existingAppointment is null)
            return;

        existingAppointment.AppointmentDate = appointment.AppointmentDate;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken = default)
    {
        var appointment = await context.Appointments
            .FirstOrDefaultAsync(
                a => a.PatientId == patientId &&
                     a.DoctorId == doctorId,
                cancellationToken);

        if (appointment is null)
            return;

        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync(cancellationToken);
    }
}
