using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Pagination;

public class PaginatedList<T> : List<T>
{
    private PaginatedList(Page page, long totalCount)
    {
        Page = page;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)Page.Size);
    }

    public Page Page { get; set; } = new Page();
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => Page.Number * Page.Size < TotalCount;
    public bool HasPreviousPage => Page.Number > 1;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, Page page, CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync();
        var data = source
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size);

        var paginatedList = new PaginatedList<T>(page, totalCount);
        await foreach(T item in data.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            paginatedList.Add(item);
        }
        return paginatedList;
    }

    public static PaginatedList<T> Create(IEnumerable<T> source, Page page)
    {
        var totalCount = source.Count();
        var data = source
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size);

        var paginatedList = new PaginatedList<T>(page, totalCount);
        foreach(var item in data)
        {
            paginatedList.Add(item);
        }
        return paginatedList;
    }
}
