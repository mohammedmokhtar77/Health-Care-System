using Health_Care_System.API.DTOs.Doctors;
using Health_Care_System.Domain.Entities.Common.Pagination;
using Health_Care_System.Domain.Entities.Common.Results;

namespace Health_Care_System.API.Services.Interfaces;

public interface IDoctorService
{
    public Task<GenericResult<PaginatedList<DoctorDto>>>
        GetAllDoctorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    
    public Task<GenericResult<DoctorDto>> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    public Task<GenericResult<DoctorDto>> CreateAsync(CreateDoctorDto createDoctorDto, CancellationToken cancellationToken);
    
    public Task<Result> Update(int id , UpdateDoctorDto updateDoctorDto, CancellationToken cancellationToken);
    
    public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
}