using AutoMapper;
using Health_Care_System.API.DTOs.Doctors;
using Health_Care_System.API.Services.Interfaces;
using Health_Care_System.Domain.Entities.Common.Pagination;
using Health_Care_System.Domain.Entities.Common.Results;
using Health_Care_System.Domain.Repositories.Interfaces;

namespace Health_Care_System.API.Services.Implementations;

public class DoctorServices 
    (IDoctorRepository repository, IMapper mapper) : IDoctorService
{
    
    public async Task<GenericResult<PaginatedList<DoctorDto>>> 
        GetAllDoctorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
       var doctors = await repository
           .GetAllAsync(pageNumber, pageSize, cancellationToken);
       
       var doctorsDto = mapper.Map<IReadOnlyList<DoctorDto>>(doctors);
       
       var result = new PaginatedList<DoctorDto>(
           doctorsDto,
           doctors.TotalCount,
           doctors.PageNumber,
           doctors.PageSize);
       
       return GenericResult<PaginatedList<DoctorDto>>.Success(result);
    }

    public async Task<GenericResult<DoctorDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var doctor = await repository.GetByIdAsync(id, cancellationToken);
        
        if (doctor is null)
            return GenericResult<DoctorDto>.Failure("Doctor not found");
        
        var doctorDto = mapper.Map<DoctorDto>(doctor);
        
        return GenericResult<DoctorDto>.Success(doctorDto);
        
    }

    public async Task<GenericResult<DoctorDto>> CreateAsync(CreateDoctorDto createDoctorDto, CancellationToken cancellationToken)
    {
        var doctor = mapper.Map<Doctor>(createDoctorDto);
        
        await  repository.AddAsync(doctor, cancellationToken);
        
        var doctorDto = mapper.Map<DoctorDto>(doctor);
        
        return GenericResult<DoctorDto>.Success(doctorDto);
    }

    public async Task<Result> Update(int id, UpdateDoctorDto updateDoctorDto, CancellationToken cancellationToken)
    {
        var doctor = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (doctor is null)
            return Result.Failure("Doctor not found.");

        mapper.Map(updateDoctorDto, doctor);

        var updated = await repository.UpdateAsync(
            doctor,
            cancellationToken);

        if (!updated)
            return Result.Failure("Failed to update doctor.");

        return Result.Success();
    }
    

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var doctor = await repository.GetByIdAsync(id, cancellationToken);

        if (doctor is null)
            return Result.Failure("Doctor Not Found.");

        var deleted = await repository.DeleteAsync(id, cancellationToken);
        
        if(!deleted)
            return Result.Failure("Failed to delete doctor.");
        
        return Result.Success();
    }
}