using Health_Care_System.API.DTOs.Patients;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController (IPatientRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientDto>>> 
        GetAllPatients(CancellationToken cancellationToken)
    {
        var patients = await repository.GetAllAsync(cancellationToken);
        var patientsDto = patients.Select(p => new PatientDto(
            p.Id , p.Name , p.DateOfBirth
            )).ToList();
        return Ok(patientsDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientDto>> GetById(int id , CancellationToken cancellationToken)
    {
        var patient = await repository.GetByIdAsync(id, cancellationToken);
        if (patient is null)
            return NotFound();
        var patientDto = new PatientDto(patient.Id, patient.Name, patient.DateOfBirth);
        return Ok(patientDto);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create(
         CreatePatientDto patientDto
        , CancellationToken cancellationToken
    )
    {
        var patient = new Patient()
        {
            Name = patientDto.Name,
            DateOfBirth = patientDto.DateOfBirth
        };
        await repository.AddAsync(patient, cancellationToken);
        var result = new PatientDto(patient.Id, patient.Name, patient.DateOfBirth);
        
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id, UpdatePatientDto patientDto, CancellationToken cancellationToken
    )
    {
        var patient = await repository.GetByIdAsync(id, cancellationToken);
        if (patient is null)
            return NotFound();
        patient.Name = patientDto.Name;
        patient.DateOfBirth = patientDto.DateOfBirth;
        await repository.UpdateAsync(patient, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var patient = await repository.GetByIdAsync(id, cancellationToken);

        if (patient is null)
            return NotFound();
        await repository.DeleteAsync(id , cancellationToken);
        
        return NoContent();
    }
}