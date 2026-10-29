using Health_Care_System.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Health_Care_System.Infrastructure.Persistence.Interceptors;

public class ModifiedDateInceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyDeleteDate(eventData);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyDeleteDate(eventData);
        return ValueTask.FromResult(result);
    }

    private void ApplyDeleteDate(DbContextEventData eventData)
    {
        if (eventData.Context is null)
            return;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State != EntityState.Modified)
                continue;
            
            entry.Entity.ModifiedDate = DateTime.UtcNow;            
        }
    }
}