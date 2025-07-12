using AutoMapper;
using Inventory.Product.API.Entities;
using Inventory.Product.API.Extensions;
using Inventory.Product.API.Repositories.Abstraction;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.Product.API.Services;

public class InventoryService : MongoDbRepository<InventoryEntry>, IInventoryService
{
    private readonly IMapper _mapper;
    public InventoryService(IMapper mapper,
        IMongoClient client, IOptions<DatabaseSettings> option) : base(client, option)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNo(string itemNo)
    {
        var entities = await FindAll()
            .Find(x => x.ItemNo.Equals(itemNo))
            .ToListAsync();
        var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);
        return result;
    }

    public async Task<PageList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query)
    {
        var filterSearchTerm = Builders<InventoryEntry>.Filter.Empty;
        var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo());

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            filterSearchTerm = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchTerm);
        }

        var andFilter = filterItemNo & filterSearchTerm;
        var pagedList = await Collection.Find(andFilter)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Limit(query.PageSize)
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(pagedList);

        return new PageList<InventoryEntryDto>(result, pagedList.Count, query.PageNumber, query.PageSize);
    }

    public async Task<InventoryEntryDto> GetById(string id)
    {
        var filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);
        var entity = await FindAll()
            .Find(filter)
            .FirstOrDefaultAsync();
        var result = _mapper.Map<InventoryEntryDto>(entity);
        return result;
    }

    public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
    {
        var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
        {
            ItemNo = model.ItemNo,
            Quantity = model.Quantity,
            DocumentType = model.DocumentType
        };

        await CreateAsync(itemToAdd);
        var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
        return result;
    }
}
