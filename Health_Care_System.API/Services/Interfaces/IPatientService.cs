using Health_Care_System.API.DTOs.Patients;
using Health_Care_System.Domain.Entities.Common.Pagination;
using Health_Care_System.Domain.Entities.Common.Results;

namespace Health_Care_System.API.Services.Interfaces;

public interface IPatientService
{
    Task<GenericResult<PaginatedList<PatientDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<GenericResult<PatientDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<GenericResult<PatientDto>> CreateAsync(
        CreatePatientDto dto,
        CancellationToken cancellationToken);

    Task<Result> UpdateAsync(
        int id,
        UpdatePatientDto dto,
        CancellationToken cancellationToken);

    Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken);
}