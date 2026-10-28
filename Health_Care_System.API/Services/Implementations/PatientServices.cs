using AutoMapper;
using Health_Care_System.API.DTOs.Patients;
using Health_Care_System.API.Services.Interfaces;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Entities.Common.Pagination;
using Health_Care_System.Domain.Entities.Common.Results;
using Health_Care_System.Domain.Repositories.Interfaces;

namespace Health_Care_System.API.Services.Implementations;

public class PatientServices(
    IPatientRepository repository,
    IMapper mapper) : IPatientService
{
    public async Task<GenericResult<PaginatedList<PatientDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        if (pageNumber < 1 || pageSize < 1)
            return GenericResult<PaginatedList<PatientDto>>.Failure(
                "Page number and page size must be greater than zero.");

        var patients = await repository.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        var patientsDto = mapper.Map<IReadOnlyList<PatientDto>>(
            patients.Items);

        var result = new PaginatedList<PatientDto>(
            patientsDto,
            patients.TotalCount,
            patients.PageNumber,
            patients.PageSize);

        return GenericResult<PaginatedList<PatientDto>>.Success(result);
    }

    public async Task<GenericResult<PatientDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            return GenericResult<PatientDto>.Failure(
                "Patient ID must be greater than zero.");

        var patient = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (patient is null)
            return GenericResult<PatientDto>.Failure(
                "Patient not found.");

        var patientDto = mapper.Map<PatientDto>(patient);

        return GenericResult<PatientDto>.Success(patientDto);
    }

    public async Task<GenericResult<PatientDto>> CreateAsync(
        CreatePatientDto dto,
        CancellationToken cancellationToken)
    {
        var patient = mapper.Map<Patient>(dto);

        await repository.AddAsync(patient, cancellationToken);

        var patientDto = mapper.Map<PatientDto>(patient);

        return GenericResult<PatientDto>.Success(patientDto);
    }

    public async Task<Result> UpdateAsync(
        int id,
        UpdatePatientDto dto,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            return Result.Failure(
                "Patient ID must be greater than zero.");

        var patient = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (patient is null)
            return Result.Failure("Patient not found.");

        mapper.Map(dto, patient);

        var updated = await repository.UpdateAsync(
            patient,
            cancellationToken);

        if (!updated)
            return Result.Failure("Failed to update patient.");

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            return Result.Failure(
                "Patient ID must be greater than zero.");

        var patient = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (patient is null)
            return Result.Failure("Patient not found.");

        var deleted = await repository.DeleteAsync(
            id,
            cancellationToken);

        if (!deleted)
            return Result.Failure("Failed to delete patient.");

        return Result.Success();
    }
}
