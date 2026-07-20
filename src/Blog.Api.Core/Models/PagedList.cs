namespace Blog.Api.Core.Models;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;

    public PagedList(IReadOnlyList<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (count + pageSize - 1) / pageSize;
    }
}

public static class PagedList
{
    public static PagedList<T> Create<T>(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static PagedList<T> Create<T>(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var enumerable = source.ToList();
        var count = enumerable.Count;
        var items = enumerable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
