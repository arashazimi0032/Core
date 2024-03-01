namespace Core.Domain.Pagination;

public static class CleanPaginationExtensions
{
    public static async Task<CleanPaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, CleanPage page, CancellationToken cancellationToken = default)
    {
        return await CleanPaginatedList<T>.CreateAsync(source, page, cancellationToken);
    }
    public static CleanPaginatedList<T> ToPaginatedList<T>(this IEnumerable<T> source, CleanPage page)
    {
        return CleanPaginatedList<T>.Create(source, page);
    }
}
