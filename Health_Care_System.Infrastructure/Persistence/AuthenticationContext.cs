using Health_Care_System.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence;

public class AuthenticationContext : IdentityDbContext<ApplicationUser>
{
    public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
        : base(options)
    {}
}