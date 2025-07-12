using Inventory.Product.API.Entities;
using Inventory.Product.API.Repositories.Abstraction;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.Product.API.Services.Interfaces;

public interface IInventoryService : IMongoDbRepositoryBase<InventoryEntry>
{
    Task<IEnumerable<InventoryEntryDto>> GetAllByItemNo(string itemNo);

    Task<PageList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query);

    Task<InventoryEntryDto> GetById(string id);

    Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
}
