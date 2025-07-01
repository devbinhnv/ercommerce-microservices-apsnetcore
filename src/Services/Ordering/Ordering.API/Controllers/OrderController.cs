using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Shared.SeedWork;
using System.Net;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController(
    IMediator mediator) 
    : ControllerBase
{
    private static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
    }
    
    [HttpGet("{username}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(ApiResult<List<OrderDto>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<List<OrderDto>>>> GetOrderByUserName(string username)
    {
        var query = new GetOrdersQuery(username);
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
