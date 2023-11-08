using Microsoft.EntityFrameworkCore;
using Web.ControllerApi.Template.Models;

namespace Web.ControllerApi.Template.Extensions;

public static class PagedListExtension
{
    public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> source, int page, int pageSize)
        where T : class
    {
        var count = await source.CountAsync();
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, page, pageSize, count);
    }

    public static PagedList<T> ToPagedList<T>(this List<T> source, int page, int pageSize, int totalCount)
        where T : class
    {
        return new PagedList<T>(source, page, pageSize, totalCount);
    }
}