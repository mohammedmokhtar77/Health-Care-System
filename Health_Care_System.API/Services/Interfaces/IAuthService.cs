using Health_Care_System.API.DTOs.Account;
using Health_Care_System.Domain.Entities.Common.Results;

namespace Health_Care_System.API.Services.Interfaces;

public interface IAuthService
{
    Task<GenericResult<AuthResponseDto>> RegisterAsync(
        RegisterDto dto,
        CancellationToken cancellationToken = default);

    Task<GenericResult<AuthResponseDto>> LoginAsync(
        LoginDto dto,
        CancellationToken cancellationToken = default);

    Task<GenericResult<AuthResponseDto>> CreateUserAsync(
        CreateUserDto dto,
        CancellationToken cancellationToken = default);
}
