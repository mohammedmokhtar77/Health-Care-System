namespace Health_Care_System.API.DTOs.Appointments;

public record AppointmentDto(
    int PatientId,
    string PatientName,
    int DoctorId,
    string DoctorName,
    DateTime AppointmentDate
    );