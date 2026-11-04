namespace Health_Care_System.API.DTOs.Account;

public record RegisterDto(
    string FullName,
    string Email,
    string Password);
