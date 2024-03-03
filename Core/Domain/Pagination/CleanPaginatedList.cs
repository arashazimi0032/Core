using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Pagination;

public class CleanPaginatedList<T>
{
    private CleanPaginatedList(List<T> items, CleanPage page, long totalCount)
    {
        Items = items;
        Page = page;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)Page.Size);
    }

    public List<T> Items { get; set; }
    public CleanPage Page { get; set; } = new CleanPage();
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => Page.Number * Page.Size < TotalCount;
    public bool HasPreviousPage => Page.Number > 1;

    public static async Task<CleanPaginatedList<T>> CreateAsync(IQueryable<T> source, CleanPage page, CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync();
        var items = await source
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToListAsync(cancellationToken);

        var paginatedList = new CleanPaginatedList<T>(items, page, totalCount);
        return paginatedList;
    }

    public static CleanPaginatedList<T> Create(IEnumerable<T> source, CleanPage page)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();

        var paginatedList = new CleanPaginatedList<T>(items, page, totalCount);
        return paginatedList;
    }
}
