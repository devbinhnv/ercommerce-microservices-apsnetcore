namespace Shared.SeedWork;

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
}
