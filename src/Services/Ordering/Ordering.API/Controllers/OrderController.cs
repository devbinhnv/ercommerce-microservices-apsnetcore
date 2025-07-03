using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Commands;
using Ordering.Application.Features.V1.Orders;
using Shared.SeedWork;
using System.ComponentModel.DataAnnotations;
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
        internal const string GetOrders = nameof(GetOrders);
        internal const string CreateOrder = nameof(CreateOrder);
        internal const string UpdateOrder = nameof(UpdateOrder);
        internal const string DeleteOrder = nameof(DeleteOrder);

    }

    [HttpGet("{username}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(ApiResult<List<OrderDto>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<List<OrderDto>>>> GetOrderByUserName(string username)
    {
        var query = new GetOrdersQuery(username);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost(Name = RouteNames.CreateOrder)]
    [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody]CreateOrderCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:long}", Name = RouteNames.UpdateOrder)]
    [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<long>>> UpdateOrder(
        [Required] long id, 
        [FromBody] UpdateOrderCommand command)
    {
        command.SetId(id);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}", Name = RouteNames.DeleteOrder)]
    [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
    public async Task<NoContentResult> DeleteOrder([Required] long id)
    {
        await mediator.Send(new DeleteOrderCommand(id));
        return NoContent();
    }
}
