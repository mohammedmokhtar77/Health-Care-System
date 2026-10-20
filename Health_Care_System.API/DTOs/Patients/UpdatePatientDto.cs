namespace Health_Care_System.API.DTOs.Patients;

public record UpdatePatientDto(
    string Name,
    DateTime DateOfBirth
    );