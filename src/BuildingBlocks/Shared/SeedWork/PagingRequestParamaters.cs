namespace Shared.SeedWork;

public class PagingRequestParamaters
{
    private const int maxPageSize = 50;
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageIndex
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value > 0) _pageSize = value > maxPageSize ? maxPageSize : value;

        }
    }
    public string OrderBy { get; set; }
}
