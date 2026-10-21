using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence;

public class HealthCareContext : DbContext
{
    public HealthCareContext(DbContextOptions<HealthCareContext> options)
                : base(options){ }
    
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    public override int SaveChanges()
    {
        ApplyAuditInformation();
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        // Current Date 
        DateTimeOffset now = DateTimeOffset.Now;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = now;
                    entry.Entity.IsDeleted = false;
                    break;
                
                case EntityState.Modified:
                    entry.Property(x => x.CreatedDate)
                        .IsModified = false;
                    entry.Property(x => x.CreatedBy)
                        .IsModified = false;

                    entry.Entity.ModifiedDate = now;
                    break;
                
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = now;
                    break;
            }
        }    
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(HealthCareContext).Assembly);
    }
}