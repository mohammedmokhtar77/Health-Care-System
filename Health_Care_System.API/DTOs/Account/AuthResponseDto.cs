namespace Health_Care_System.API.DTOs.Account;

public record AuthResponseDto(
    string UserId,
    string FullName,
    string Email,
    string Role,
    string Token);