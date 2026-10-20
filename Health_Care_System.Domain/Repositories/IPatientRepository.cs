using Health_Care_System.Domain.Entities;

namespace Health_Care_System.Domain.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<IReadOnlyList<Patient>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);

    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}