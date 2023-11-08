namespace Web.ControllerApi.Template.Models;

public class BaseFilter
{
    private int _page { get; set; } = 1;

    public int Page
    {
        get => _page;
        set => _page = value > 0 ? value : 1;
    }

    private int _pageSize { get; set; } = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 ? value : 10;
    }
}