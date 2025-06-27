using Basket.API.Repositories;
using Basket.API.Repositories.interfaces;
using Contracts.Common;
using Infrastructure.Common;

namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfratuctures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBasketRepository, BasketRepository>()
                .AddTransient<ISerializeService, SerializeService>();

        services.AddRedisCache(configuration);
        return services;
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetSection("ConnectionStrings")["RedisConnectionString"];
        ArgumentNullException.ThrowIfNullOrEmpty(redisConnectionString, nameof(redisConnectionString));

        services.AddStackExchangeRedisCache(opt => opt.Configuration = redisConnectionString);
        return services;
    }
}
