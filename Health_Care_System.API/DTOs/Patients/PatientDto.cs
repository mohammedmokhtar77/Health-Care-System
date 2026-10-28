namespace Health_Care_System.API.DTOs.Patients;

public record PatientDto
(
    int Id,
    string FullName,
    DateTime DateOfBirth
);