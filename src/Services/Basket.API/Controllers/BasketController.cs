using Basket.API.Entities;
using Basket.API.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static System.Net.WebRequestMethods;

namespace Basket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketController(IBasketRepository basketRepository)
    : ControllerBase
{
    [HttpGet("{username}", Name ="GetBasket")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> GetBasketByUsername([Required]string username)
    {
        var result = await basketRepository.GetBasketByUserName(username);
        return Ok(result ?? new Cart());
    }

    [HttpPost(Name = "CreateOrUpdateBasket")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> CreateOrUpdateBasket([FromBody] Cart cart)
    {
        var option = new DistributedCacheEntryOptions
        {
            //Expires in 1 hour
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(5),
        };
        var result = await basketRepository.CreateOrUpdate(cart, option);
        return Ok(result);
    }

    [HttpDelete("{username}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteBasketByUserName([Required]string username)
    {
        var result = await basketRepository.DeleteBasketFromUserName(username);
        return Ok(result);
    }
}
