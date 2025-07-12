using Inventory.Product.API.Entities;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Product.API.Persistence;

public class InventoryDbSeed
{
    public async Task SeedDataAsync(IMongoClient mongoClient, MongoDbSettings settings)
    {
        var databaseName = settings.DatabaseName;
        var database = mongoClient.GetDatabase(databaseName);

        var inventoryCollections = database.GetCollection<InventoryEntry>("InventoryEntries");
        if(await inventoryCollections.EstimatedDocumentCountAsync() == 0)
        {
            await inventoryCollections.InsertManyAsync(GetPreconfiguredInventoryEntries());
        }
    }

    private IEnumerable<InventoryEntry> GetPreconfiguredInventoryEntries()
    {
        return new List<InventoryEntry>()
        {
            new()
            {
                Quantity = 10,
                DocumentNo = Guid.NewGuid().ToString(),
                ItemNo = "PRD001",
                ExternalDocumentNo = Guid.NewGuid().ToString(),
                DocumentType = Shared.Enums.EDocumentType.Purchase,
            },
            new()
            {
                Quantity = 5,
                DocumentNo = Guid.NewGuid().ToString(),
                ItemNo = "PRD002",
                ExternalDocumentNo = Guid.NewGuid().ToString(),
                DocumentType = Shared.Enums.EDocumentType.Purchase,
            }
        };
    }
}
