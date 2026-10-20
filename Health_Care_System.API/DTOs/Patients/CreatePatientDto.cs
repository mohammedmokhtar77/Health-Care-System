namespace Health_Care_System.API.DTOs.Patients;

public record CreatePatientDto(
    string Name,
    DateTime DateOfBirth
    );