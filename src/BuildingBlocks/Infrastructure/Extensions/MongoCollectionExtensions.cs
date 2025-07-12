using Infrastructure.Common.Models;
using MongoDB.Driver;

namespace Infrastructure.Extensions;

public static class MongoCollectionExtensions
{
    public static async Task<PageList<TDestination>> PaginatedPageAsync<TDestination>(
        this IMongoCollection<TDestination> collection,
        FilterDefinition<TDestination> filter,
        int pageIndex, int pageSize) where TDestination : class
    {
        return await PageList<TDestination>.ToPagedList(collection, filter, pageIndex, pageSize);
    }
}
