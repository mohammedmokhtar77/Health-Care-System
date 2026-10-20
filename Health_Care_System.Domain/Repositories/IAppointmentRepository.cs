namespace Health_Care_System.Domain.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task<IReadOnlyList<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Appointment?> GetByIdAsync(int patientId, int doctorId, CancellationToken cancellationToken = default);

    Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);

    Task DeleteAsync(int patientId, int doctorId, CancellationToken cancellationToken = default);
}