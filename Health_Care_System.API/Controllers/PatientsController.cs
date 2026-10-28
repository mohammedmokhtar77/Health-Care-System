using AutoMapper;
using Health_Care_System.API.DTOs.Patients;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Entities.Common.Pagination;
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientRepository repository , IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> 
        GetAllPatients(
            int pageNumber, int pageSize,
            CancellationToken cancellationToken)
    {
        var patients = await 
            repository.GetAllAsync(
                pageNumber
                , pageSize, 
                cancellationToken);
        
        var patientsDto = mapper.Map<IReadOnlyList<PatientDto>>
            (patients.Items);
        
        var result = new PaginatedList<PatientDto>(
            patientsDto,
             patients.TotalCount
            , patients.PageNumber 
            , patients.PageSize);
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientDto>>
        GetById(int id , CancellationToken cancellationToken)
    {
        var patient = await repository.GetByIdAsync(id, cancellationToken);
        
        if (patient is null)
            return NotFound();
        
        var patientDto = mapper.Map<PatientDto>(patient);
        
        return Ok(patientDto);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create(
         CreatePatientDto patientDto
        , CancellationToken cancellationToken
    )
    {
        
        var patient = mapper.Map<Patient>(patientDto);
        
        await repository.AddAsync(patient, cancellationToken);
        
        var result = mapper.Map<PatientDto>(patient);
        
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
        
        mapper.Map(patientDto, patient);
        
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