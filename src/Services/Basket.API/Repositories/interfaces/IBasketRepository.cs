using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories.interfaces;

public interface IBasketRepository
{
    Task<Cart?> GetBasketByUserName(string userName);
    Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions? option = default);
    Task<bool> DeleteBasketFromUserName(string userName);
}
