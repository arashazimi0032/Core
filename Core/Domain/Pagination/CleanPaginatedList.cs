using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Pagination;

public class CleanPaginatedList<T> : List<T>
{
    private CleanPaginatedList(CleanPage page, long totalCount)
    {
        Page = page;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)Page.Size);
    }

    public CleanPage Page { get; set; } = new CleanPage();
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => Page.Number * Page.Size < TotalCount;
    public bool HasPreviousPage => Page.Number > 1;

    public static async Task<CleanPaginatedList<T>> CreateAsync(IQueryable<T> source, CleanPage page, CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync();
        var data = source
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size);

        var paginatedList = new CleanPaginatedList<T>(page, totalCount);
        await foreach(T item in data.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            paginatedList.Add(item);
        }
        return paginatedList;
    }

    public static CleanPaginatedList<T> Create(IEnumerable<T> source, CleanPage page)
    {
        var totalCount = source.Count();
        var data = source
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size);

        var paginatedList = new CleanPaginatedList<T>(page, totalCount);
        foreach(var item in data)
        {
            paginatedList.Add(item);
        }
        return paginatedList;
    }
}
