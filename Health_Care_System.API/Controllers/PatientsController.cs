using Health_Care_System.API.DTOs.Patients;
using Health_Care_System.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(
    IPatientService patientService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPatients(
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await patientService.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { result.Error });

        return Ok(result.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await patientService.GetByIdAsync(
            id,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Patient not found.")
                return NotFound(new { result.Error });

            return BadRequest(new { result.Error });
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePatientDto dto,
        CancellationToken cancellationToken)
    {
        var result = await patientService.CreateAsync(
            dto,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { result.Error });

        var patient = result.Data!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = patient.Id },
            patient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdatePatientDto dto,
        CancellationToken cancellationToken)
    {
        var result = await patientService.UpdateAsync(
            id,
            dto,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Patient not found.")
                return NotFound(new { result.Error });

            return BadRequest(new { result.Error });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await patientService.DeleteAsync(
            id,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Patient not found.")
                return NotFound(new { result.Error });

            return BadRequest(new { result.Error });
        }

        return NoContent();
    }
}
