using AutoMapper;
using Health_Care_System.API.DTOs.Appointments;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(
    IAppointmentRepository repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAppointments(
        CancellationToken cancellationToken)
    {
        var appointments = await repository.GetAllAsync(cancellationToken);

        var appointmentsDto = mapper.Map<IReadOnlyList<AppointmentDto>>(
            appointments);

        return Ok(appointmentsDto);
    }

    [HttpGet("{patientId:int}/{doctorId:int}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (appointment is null)
            return NotFound();

        var appointmentDto = mapper.Map<AppointmentDto>(appointment);

        return Ok(appointmentDto);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(
        CreateAppointmentDto appointmentDto,
        CancellationToken cancellationToken)
    {
        var appointment = mapper.Map<Appointment>(appointmentDto);

        await repository.AddAsync(appointment, cancellationToken);

        // Reload the appointment to retrieve Patient and Doctor navigation data.
        var createdAppointment = await repository.GetByIdAsync(
            appointment.PatientId,
            appointment.DoctorId,
            cancellationToken);

        if (createdAppointment is null)
            return Problem("The appointment was created but could not be retrieved.");

        var response = mapper.Map<AppointmentDto>(createdAppointment);

        return CreatedAtAction(
            nameof(GetAppointment),
            new
            {
                patientId = createdAppointment.PatientId,
                doctorId = createdAppointment.DoctorId
            },
            response);
    }

    [HttpPut("{patientId:int}/{doctorId:int}")]
    public async Task<IActionResult> UpdateAppointment(
        int patientId,
        int doctorId,
        UpdateAppointmentDto appointmentDto,
        CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (appointment is null)
            return NotFound();

        mapper.Map(appointmentDto, appointment);

        await repository.UpdateAsync(appointment, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{patientId:int}/{doctorId:int}")]
    public async Task<IActionResult> DeleteAppointment(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (appointment is null)
            return NotFound();

        await repository.DeleteAsync(patientId, doctorId, cancellationToken);

        return NoContent();
    }
}
