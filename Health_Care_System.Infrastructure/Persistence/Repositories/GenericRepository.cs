using Health_Care_System.Domain.Entities.Common;
using Health_Care_System.Domain.Entities.Common.Pagination;
using Health_Care_System.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence.Repositories;

public class GenericRepository <T , TKey>(HealthCareContext context) 
    : IGenericRepository<T , TKey> where T : AuditableEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    public async Task<PaginatedList<T>> 
        GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        
        var query = _dbSet.AsNoTracking();
        
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await
            query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity =await _dbSet
            .FindAsync(id, cancellationToken);
        return entity;
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return true;       
    }

    public async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        
        if (entity is null) return false;
        
        _dbSet.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}