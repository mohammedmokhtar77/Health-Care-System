namespace Health_Care_System.API.DTOs.Account;

public record CreateUserDto(
    string FullName,
    string Email,
    string Password,
    string Role);
