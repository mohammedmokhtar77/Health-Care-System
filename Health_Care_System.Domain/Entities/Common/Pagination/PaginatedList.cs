namespace Health_Care_System.Domain.Entities.Common.Pagination;

public class PaginatedList<T>
{
    public IReadOnlyList<T> Items { get; }

    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }

    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedList(
        IReadOnlyList<T> items,
        int totalCount,
        int pageNumber,
        int pageSize)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (totalCount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalCount));

        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber));

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}