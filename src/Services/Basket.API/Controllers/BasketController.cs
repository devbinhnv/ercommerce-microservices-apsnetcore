using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.interfaces;
using EvenBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketController(
    IBasketRepository basketRepository,
    IPublishEndpoint publishEndpoint,
    IMapper mapper)
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

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        var basket = await basketRepository.GetBasketByUserName(basketCheckout.UserName);
        if (basket == null)
            return NotFound();

        // publish event checkout to event bus
        var eventMessage = mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;
        await publishEndpoint.Publish(eventMessage);

        // remove basket after send message
        await basketRepository.DeleteBasketFromUserName(basketCheckout.UserName);
        return Accepted();
    }
}
