using Microsoft.AspNetCore.Identity;

namespace Health_Care_System.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty; 
}