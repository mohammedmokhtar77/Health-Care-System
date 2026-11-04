using Health_Care_System.API.DTOs.Account;
using Health_Care_System.API.Services.Interfaces;
using Health_Care_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(dto, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(new { result.Error });
        return Ok(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(dto, cancellationToken);
        if (!result.IsSuccess)
            return Unauthorized(new { result.Error });
        return Ok(result.Data);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.CreateUserAsync(dto, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(new { result.Error });
        return Ok(result.Data);
    }
}
