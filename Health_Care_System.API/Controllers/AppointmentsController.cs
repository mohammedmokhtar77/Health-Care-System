using Health_Care_System.API.DTOs.Appointments;
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(IAppointmentRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAppointments(CancellationToken cancellationToken)
    {
        var appointments = await repository.GetAllAsync(cancellationToken);
        var appointmentsDto = appointments.Select(a =>
            new AppointmentDto(a.PatientId, a.Patient.Name, a.DoctorId, a.Doctor.Name, a.AppointmentDate));
        return Ok(appointmentsDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int patientId,int doctorId , CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(patientId, doctorId, cancellationToken);
        if(appointment is null)
            return NotFound();
        
        var appointmentDto = new AppointmentDto(appointment.PatientId, appointment.Patient.Name, appointment.DoctorId,
            appointment.Doctor.Name, appointment.AppointmentDate);
        return Ok(appointmentDto);
    }

    [HttpPost]
    public async Task<ActionResult<CreateAppointmentDto>> CreateAppointment(CreateAppointmentDto appointmentDto,
        CancellationToken cancellationToken)
    {
        var appointment = new Appointment()
        {
            PatientId = appointmentDto.PatientId,
            DoctorId = appointmentDto.DoctorId,
            AppointmentDate = appointmentDto.AppointmentDate
        };
        await repository.AddAsync(appointment, cancellationToken);

        var result =
            await repository.GetByIdAsync(appointment.PatientId, appointment.DoctorId,cancellationToken);
        
        var response = new AppointmentDto(
            appointment.PatientId,
            result?.Patient?.Name ?? string.Empty,
            appointment.DoctorId,
            result?.Doctor?.Name ?? string.Empty,
            appointment.AppointmentDate
        );
        
        return CreatedAtAction(nameof(GetAppointment), new { PatientId = appointment.PatientId , DoctorId = appointment.DoctorId }, response);
    }

    [HttpPut("{patientId:int}/{doctorId:int}")]
    public async Task<IActionResult> UpdateAppointment(int patientId,int doctorId, UpdateAppointmentDto appointmentDto,
        CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(patientId, doctorId, cancellationToken);
        if (appointment is null)
            return NotFound();
        
        appointment.AppointmentDate = appointmentDto.AppointmentDate;
        await repository.UpdateAsync(appointment, cancellationToken);
        
        return NoContent();
    }

    [HttpDelete("{patientId:int}/{doctorId:int}")]
    public async Task<IActionResult> DeleteAppointment(int patientId, int doctorId, CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(patientId, doctorId, cancellationToken);
        if (appointment is null)
            return NotFound();
        await repository.DeleteAsync(patientId, doctorId, cancellationToken);
        return NoContent();
    }
}