namespace Health_Care_System.Domain.Repositories.Interfaces;

public interface IDoctorRepository
{
    Task<IReadOnlyList<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default);

    Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}