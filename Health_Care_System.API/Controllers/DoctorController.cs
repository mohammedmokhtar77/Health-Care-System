using AutoMapper;
using Health_Care_System.API.DTOs.Doctors;
using Health_Care_System.Domain.Entities.Common.Pagination;
using Health_Care_System.Domain.Entities.Common.Results;
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class 
    DoctorController(IDoctorRepository repository , IMapper mapper ) 
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllDoctors(
        int pageNumber, int pageSize 
        , CancellationToken cancellationToken
    )
    {
        var doctors = await repository
            .GetAllAsync(pageNumber, pageSize, cancellationToken);
        
        var doctorsDto = mapper.Map<IReadOnlyList<DoctorDto>>(doctors.Items);

        var result = new 
            PaginatedList<DoctorDto>(
                doctorsDto,
                doctors.TotalCount
                ,  doctors.PageNumber
                , doctors.PageSize);

        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id, CancellationToken cancellationToken)
    {
        var doctor =await repository.GetByIdAsync(id, cancellationToken);
        
        if (doctor is null)
            return NotFound();
        
        var doctorDto = mapper.Map<DoctorDto>(doctor);
        
        return Ok(doctorDto);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> Create(CreateDoctorDto doctorDto , CancellationToken cancellationToken)
    {
        var doctor = mapper.Map<Doctor>(doctorDto);
        
        await repository.AddAsync(doctor, cancellationToken);

        var result = mapper.Map<DoctorDto>(doctor);
        
        return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDoctorDto dto, CancellationToken cancellationToken)
    {
        var doctor = await repository.GetByIdAsync(id, cancellationToken);

        if (doctor is null)
            return NotFound();

        mapper.Map(dto, doctor);

        await repository.UpdateAsync(doctor, cancellationToken);

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var doctor = await repository.GetByIdAsync(id, cancellationToken);

        if (doctor is null)
            return NotFound();

        await repository.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}