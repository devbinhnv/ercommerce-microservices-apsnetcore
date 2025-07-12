using MongoDB.Driver;
using Shared.SeedWork;

namespace Infrastructure.Common.Models;

public class PageList<T> : List<T>
{
    private readonly MetaData _metaData;

    public PageList(IEnumerable<T> items, long totalItems, int pageNumber, int pageSize)
    {
        _metaData = new MetaData
        {
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int) Math.Ceiling(totalItems / (double) pageSize)
        };
        AddRange(items);
    }

    public MetaData GetMetaData()
    {
        return _metaData;
    }

    public static async Task<PageList<T>> ToPagedList(IMongoCollection<T> source,
        FilterDefinition<T> filter, int pageIndex, int pageSize)
    {
        var count = await source.Find(filter).CountDocumentsAsync();
        var items = await source.Find(filter)
            .Skip((pageIndex - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PageList<T>(items, count, pageIndex, pageSize);
    }
}
