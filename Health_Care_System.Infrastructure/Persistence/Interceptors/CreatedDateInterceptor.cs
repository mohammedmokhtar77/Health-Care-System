using Health_Care_System.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Health_Care_System.Infrastructure.Persistence.Interceptors;

public class CreatedDateInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyCreatedDate(eventData);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyCreatedDate(eventData);
        return ValueTask.FromResult(result);
    }

    private void ApplyCreatedDate(DbContextEventData eventData)
    {
        if (eventData.Context is null)
            return;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State!= EntityState.Added)
                continue;
            
            entry.Entity.CreatedDate = DateTime.UtcNow;
        }
    }
}