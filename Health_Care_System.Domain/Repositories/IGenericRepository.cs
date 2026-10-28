using Health_Care_System.Domain.Entities.Common;
using Health_Care_System.Domain.Entities.Common.Pagination;

namespace Health_Care_System.Domain.Repositories;

public interface IGenericRepository<T , TKey> 
    where T : AuditableEntity
{
    Task<PaginatedList<T>> 
        GetAllAsync(int pageNumber, int pageSize , CancellationToken cancellationToken);

    Task<T?> 
        GetByIdAsync(TKey id, CancellationToken cancellationToken);
    
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);
    
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken);
    
}