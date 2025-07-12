using Shared.SeedWork;

namespace Shared.DTOs.Inventory;

public class GetInventoryPagingQuery : PagingRequestParamaters
{
    public string ItemNo () => _itemNo;

    private string _itemNo;

    public void SetItemNo(string itemNo) => _itemNo = itemNo;

    public string SearchTerm { get; set; }
}
