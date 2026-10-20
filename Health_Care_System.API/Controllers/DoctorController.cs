using Health_Care_System.API.DTOs.Doctors;
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorController(IDoctorRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DoctorDto>>> GetAllDoctors(CancellationToken cancellationToken)
    {
        var doctors = await repository.GetAllAsync(cancellationToken);
        var doctorsDto = doctors.Select(
                d => new DoctorDto(
                    d.Id, d.Name, d.Specialization))
            .ToList();
        
        return Ok(doctorsDto);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id, CancellationToken cancellationToken)
    {
        var doctor =await repository.GetByIdAsync(id, cancellationToken);
        if (doctor is null)
            return NotFound();
        var doctorDto = new DoctorDto(doctor.Id, doctor.Name, doctor.Specialization);
        return Ok(doctorDto);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> Create(CreateDoctorDto doctorDto , CancellationToken cancellationToken)
    {
        var doctor = new Doctor() { Name = doctorDto.Name, Specialization = doctorDto.Specialization };
        await repository.AddAsync(doctor, cancellationToken);

        var result = new DoctorDto(doctor.Id, doctor.Name, doctor.Specialization);
        return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDoctorDto dto, CancellationToken cancellationToken)
    {
        var doctor = await repository.GetByIdAsync(id, cancellationToken);

        if (doctor is null)
            return NotFound();

        doctor.Name = dto.Name;
        doctor.Specialization = dto.Specialization;

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