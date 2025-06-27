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
        var basket = await redisCacheService.GetStringAsync(userName);
        return string.IsNullOrEmpty(basket) ? null : 
            serializeService.Deserialize<Cart>(basket);
    }

    public async Task<bool> DeleteBasketFromUserName(string userName)
    {
        try
        {
            await redisCacheService.RemoveAsync(userName);
            return true;
        }
        catch(Exception e)
        {
            logger.Error($"DeleteBasketFromUserName: ${e.Message}"); 
            throw;
        }
    }

    public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions? options = default)
    {
        if(options is null)
        {
            await redisCacheService.SetStringAsync(cart.UserName, serializeService.Seriallize(cart));
        }
        else
        {
            await redisCacheService.SetStringAsync(cart.UserName, serializeService.Seriallize(cart), options);
        }
        return await GetBasketByUserName(cart.UserName);
    }
}
