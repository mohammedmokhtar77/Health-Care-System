using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Health_Care_System.API.DTOs.Account;
using Health_Care_System.API.Services.Interfaces;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Entities.Common.Results;
using Health_Care_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Health_Care_System.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<GenericResult<AuthResponseDto>> RegisterAsync(
        RegisterDto dto,
        CancellationToken cancellationToken)
    {
        return await CreateUserInRoleAsync(dto.FullName, dto.Email, dto.Password,
            Roles.Patient, cancellationToken);
    }

    public async Task<GenericResult<AuthResponseDto>> CreateUserAsync(
        CreateUserDto dto,
        CancellationToken cancellationToken)
    {
        var requestedRole = new[] { Roles.Admin, Roles.Doctor, Roles.Receptionist, Roles.Patient }
            .FirstOrDefault(role => string.Equals(role, dto.Role, StringComparison.OrdinalIgnoreCase));

        if (requestedRole is null || !await _roleManager.RoleExistsAsync(requestedRole))
            return GenericResult<AuthResponseDto>.Failure("Requested role does not exist or is not allowed.");

        return await CreateUserInRoleAsync(dto.FullName, dto.Email, dto.Password,
            requestedRole, cancellationToken);
    }

    private async Task<GenericResult<AuthResponseDto>> CreateUserInRoleAsync(
        string fullName, string email, string password, string role,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return GenericResult<AuthResponseDto>.Failure(
                string.Join(", ", result.Errors.Select(e => e.Description)));

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return GenericResult<AuthResponseDto>.Failure(
                string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        return await GenerateTokenAsync(user);
    }

    public async Task<GenericResult<AuthResponseDto>> LoginAsync(
        LoginDto dto,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return GenericResult<AuthResponseDto>.Failure("Invalid email or password.");
        }

        return await GenerateTokenAsync(user);
    }

    private async Task<GenericResult<AuthResponseDto>> GenerateTokenAsync(
        ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();

        if (role is null)
            return GenericResult<AuthResponseDto>.Failure("User has no assigned role.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email!)
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var jwt = _configuration.GetSection("Jwt");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(jwt["DurationInMinutes"]!)),
            signingCredentials: credentials);

        return GenericResult<AuthResponseDto>.Success(
            new AuthResponseDto(
                user.Id,
                user.FullName,
                user.Email!,
                role,
                new JwtSecurityTokenHandler().WriteToken(token)));
    }
}
