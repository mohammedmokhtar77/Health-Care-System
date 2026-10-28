using Health_Care_System.API.DTOs.Appointments;
using Health_Care_System.Domain.Entities.Common.Results;

namespace Health_Care_System.API.Services.Interfaces;

public interface IAppointmentService
{
    Task<GenericResult<IReadOnlyList<AppointmentDto>>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<GenericResult<AppointmentDto>> GetByIdAsync(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken);

    Task<GenericResult<AppointmentDto>> CreateAsync(
        CreateAppointmentDto dto,
        CancellationToken cancellationToken);

    Task<Result> UpdateAsync(
        int patientId,
        int doctorId,
        UpdateAppointmentDto dto,
        CancellationToken cancellationToken);

    Task<Result> DeleteAsync(
        int patientId,
        int doctorId,
        CancellationToken cancellationToken);
}