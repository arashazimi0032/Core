namespace Core.Domain.Pagination;

public static class PaginationExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, Page page, CancellationToken cancellationToken = default)
    {
        return await PaginatedList<T>.CreateAsync(source, page, cancellationToken);
    }
    public static PaginatedList<T> ToPaginatedListAsync<T>(this IEnumerable<T> source, Page page)
    {
        return PaginatedList<T>.Create(source, page);
    }
}
