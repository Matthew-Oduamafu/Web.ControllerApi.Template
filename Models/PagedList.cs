namespace Web.ControllerApi.Template.Models;

public sealed class PagedList<T> : BasePagedList where T : class
{
    public List<T> Items { get; set; } = new();
    public List<Link> Links { get; set; } = new();

    public PagedList()
    {
    }

    public PagedList(List<T> source, int page, int pageSize, int totalCount)
    {
        Items = source;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}

public class BasePagedList
{
    private int PageIndex { get; set; } = 1;

    public int Page
    {
        get => PageIndex;
        set => PageIndex = value > 0 ? value : 1;
    }

    private int MinPageSize { get; init; } = 10;
    
    public bool HasPreviousPage => Page > 1;
    
    public bool HasNextPage => Page < TotalPages;

    public int PageSize
    {
        get => MinPageSize;
        init => MinPageSize = value > 0 ? value : 10;
    }

    public int TotalPages => (int)Math.Ceiling(Convert.ToDouble(TotalCount) / PageSize);

    public int TotalCount { get; set; }
}