using Health_Care_System.API.DTOs.Appointments;
using Health_Care_System.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(
    IAppointmentService appointmentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAppointments(
        CancellationToken cancellationToken)
    {
        var result = await appointmentService.GetAllAsync(
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { result.Error });

        return Ok(result.Data);
    }

    [HttpGet("{patientId:int}/{doctorId:int}")]
    public async Task<IActionResult> GetAppointment(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken)
    {
        var result = await appointmentService.GetByIdAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Appointment not found.")
                return NotFound(new { result.Error });

            return BadRequest(new { result.Error });
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        CreateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var result = await appointmentService.CreateAsync(
            dto,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { result.Error });

        var appointment = result.Data!;

        return CreatedAtAction(
            nameof(GetAppointment),
            new
            {
                patientId = appointment.PatientId,
                doctorId = appointment.DoctorId
            },
            appointment);
    }

    [HttpPut("{patientId:int}/{doctorId:int}")]
    public async Task<IActionResult> UpdateAppointment(
        int patientId,
        int doctorId,
        UpdateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var result = await appointmentService.UpdateAsync(
            patientId,
            doctorId,
            dto,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Appointment not found.")
                return NotFound(new { result.Error });

            return BadRequest(new { result.Error });
        }

        return NoContent();
    }

    [HttpDelete("{patientId:int}/{doctorId:int}")]
    public async Task<IActionResult> DeleteAppointment(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken)
    {
        var result = await appointmentService.DeleteAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error == "Appointment not found.")
                return NotFound(new { result.Error });

            return BadRequest(new { result.Error });
        }

        return NoContent();
    }
}
