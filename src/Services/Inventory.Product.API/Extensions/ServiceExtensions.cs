using Infrastructure.Extensions;
using Inventory.Product.API.Services;
using Inventory.Product.API.Services.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Product.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
        services.ConfigureMongoDb();
        services.AddScoped<IInventoryService, InventoryService>();

        return services;
    }

    internal static IServiceCollection ConfigureMongoDb(this IServiceCollection services)
    {
        var connectionString = services.GetMongoConnectionString();
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString))
                .AddScoped<IClientSessionHandle>(x =>
                {
                    var mongoClient = x.GetService<IMongoClient>();
                    ArgumentNullException.ThrowIfNull(mongoClient);

                    return mongoClient.StartSession();
                });
        return services;
    }

    private static string GetMongoConnectionString(this IServiceCollection services)
    {
        var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
        ArgumentNullException.ThrowIfNullOrEmpty(settings.ConnectionString, nameof(MongoDbSettings));

        var databaseName = settings.DatabaseName;
        var connectionString = string.Format("{0}/{1}?authSource=admin", 
            settings.ConnectionString, databaseName);
        return connectionString;
    }
}
