using AutoMapper;
using Health_Care_System.API.DTOs.Appointments;
using Health_Care_System.API.Services.Interfaces;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Entities.Common.Results;
using Health_Care_System.Domain.Repositories.Interfaces;

namespace Health_Care_System.API.Services.Implementations;

public class AppointmentServices(
    IAppointmentRepository repository,
    IMapper mapper) : IAppointmentService
{
    public async Task<GenericResult<IReadOnlyList<AppointmentDto>>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var appointments = await repository.GetAllAsync(
            cancellationToken);

        var appointmentsDto = mapper.Map<IReadOnlyList<AppointmentDto>>(
            appointments);

        return GenericResult<IReadOnlyList<AppointmentDto>>.Success(
            appointmentsDto);
    }

    public async Task<GenericResult<AppointmentDto>> GetByIdAsync(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken)
    {
        if (patientId <= 0 || doctorId <= 0)
        {
            return GenericResult<AppointmentDto>.Failure(
                "Patient ID and Doctor ID must be greater than zero.");
        }

        var appointment = await repository.GetByIdAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (appointment is null)
        {
            return GenericResult<AppointmentDto>.Failure(
                "Appointment not found.");
        }

        var appointmentDto = mapper.Map<AppointmentDto>(appointment);

        return GenericResult<AppointmentDto>.Success(appointmentDto);
    }

    public async Task<GenericResult<AppointmentDto>> CreateAsync(
        CreateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        if (dto.PatientId <= 0 || dto.DoctorId <= 0)
        {
            return GenericResult<AppointmentDto>.Failure(
                "Patient ID and Doctor ID must be greater than zero.");
        }

        var appointment = mapper.Map<Appointment>(dto);

        var createdAppointment = await repository.AddAsync(
            appointment,
            cancellationToken);

        // Reload to retrieve Patient and Doctor navigation properties.
        var result = await repository.GetByIdAsync(
            createdAppointment.PatientId,
            createdAppointment.DoctorId,
            cancellationToken);

        if (result is null)
        {
            return GenericResult<AppointmentDto>.Failure(
                "Appointment was created but could not be retrieved.");
        }

        var appointmentDto = mapper.Map<AppointmentDto>(result);

        return GenericResult<AppointmentDto>.Success(appointmentDto);
    }

    public async Task<Result> UpdateAsync(
        int patientId,
        int doctorId,
        UpdateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        if (patientId <= 0 || doctorId <= 0)
        {
            return Result.Failure(
                "Patient ID and Doctor ID must be greater than zero.");
        }

        var appointment = await repository.GetByIdAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (appointment is null)
        {
            return Result.Failure("Appointment not found.");
        }

        mapper.Map(dto, appointment);

        await repository.UpdateAsync(
            appointment,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken)
    {
        if (patientId <= 0 || doctorId <= 0)
        {
            return Result.Failure(
                "Patient ID and Doctor ID must be greater than zero.");
        }

        var appointment = await repository.GetByIdAsync(
            patientId,
            doctorId,
            cancellationToken);

        if (appointment is null)
        {
            return Result.Failure("Appointment not found.");
        }

        await repository.DeleteAsync(
            patientId,
            doctorId,
            cancellationToken);

        return Result.Success();
    }
}
