using Health_Care_System.API.DTOs.Doctors;
using Health_Care_System.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class 
    DoctorController( IDoctorService service) 
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllDoctors(
        int pageNumber, int pageSize 
        , CancellationToken cancellationToken
    )
    {
        var result  = await service
            .GetAllDoctorsAsync(pageNumber, pageSize, cancellationToken);
        
        if(!result.IsSuccess)
            return BadRequest(new {result.Error});
        
        return Ok(result.Data);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id, CancellationToken cancellationToken)
    {
        var result =await service
            .GetByIdAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
            return NotFound(new { result.Error });
        
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> Create(CreateDoctorDto doctorDto , CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(
            doctorDto,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { result.Error });

        var doctor = result.Data!;

        return CreatedAtAction(
            nameof(GetDoctor), 
            new { id = doctor.Id }, doctor);
    }

    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateDoctorDto dto,
        CancellationToken cancellationToken)
    {
        var result = await service.Update(
            id,
            dto,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Doctor not found.")
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
        var result = await service.DeleteAsync(
            id,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Doctor Not Found.")
                return NotFound(new { result.Error });

            return BadRequest(new { result.Error });
        }

        return NoContent(); 
    }

}