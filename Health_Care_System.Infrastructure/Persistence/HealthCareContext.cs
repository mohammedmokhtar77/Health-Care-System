using Health_Care_System.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.API.Data;

public class HealthCareContext : DbContext
{
    public HealthCareContext(DbContextOptions<HealthCareContext> options)
                : base(options){ }
    
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(HealthCareContext).Assembly);
    }
}