namespace Health_Care_System.API.DTOs.Appointments;

public record CreateAppointmentDto(
    int PatientId,
    int DoctorId,
    DateTime AppointmentDate
    );