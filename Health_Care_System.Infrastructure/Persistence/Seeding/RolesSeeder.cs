using Microsoft.AspNetCore.Identity;
using Health_Care_System.Domain.Entities.Identity;
namespace Health_Care_System.Infrastructure.Persistence.Seeding;

public static class RolesSeeder
{
   public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
   {
      string[] roles =
      [
         Roles.Admin, Roles.Doctor, Roles.Receptionist, Roles.Patient
      ];
      foreach (var role in roles)
      {
         if(!await roleManager.RoleExistsAsync(role))
               await roleManager.CreateAsync(new IdentityRole(role));
      }
   }
}
