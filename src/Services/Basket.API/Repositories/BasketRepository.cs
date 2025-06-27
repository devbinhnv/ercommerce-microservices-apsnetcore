using Basket.API.Entities;
using Basket.API.Repositories.interfaces;
using Contracts.Common;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories;

public class BasketRepository(
    IDistributedCache redisCacheService,
    ISerializeService serializeService,
    ILogger logger) : IBasketRepository
{

    public async Task<Cart?> GetBasketByUserName(string userName)
    {
        logger.Information($"START: GetBasketByUserName for username {userName}");
        var basket = await redisCacheService.GetStringAsync(userName);
        logger.Information($"END: GetBasketByUserName for username {userName}");

        return string.IsNullOrEmpty(basket) ? null : serializeService.Deserialize<Cart>(basket);
    }

    public async Task<bool> DeleteBasketFromUserName(string userName)
    {
        try
        {
            logger.Information($"START: DeleteBasketFromUserName for username {userName}");
            await redisCacheService.RemoveAsync(userName);
            logger.Information($"END: DeleteBasketFromUserName for username {userName}");

            return true;
        }
        catch(Exception e)
        {
            logger.Error($"Error DeleteBasketFromUserName: ${e.Message}"); 
            throw;
        }
    }

    public async Task<Cart> CreateOrUpdate(Cart cart, DistributedCacheEntryOptions? options = default)
    {
        logger.Information($"START: UpdateBasket for username {cart.UserName}");
        if (options is null)
        {
            await redisCacheService.SetStringAsync(cart.UserName, serializeService.Seriallize(cart));
        }
        else
        {
            await redisCacheService.SetStringAsync(cart.UserName, serializeService.Seriallize(cart), options);
        }
        logger.Information($"END: UpdateBasket for username {cart.UserName}");
        return await GetBasketByUserName(cart.UserName);
    }
}
