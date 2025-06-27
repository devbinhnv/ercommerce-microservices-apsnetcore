using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customer;

namespace Customer.API.Apis;

public static class CustomerApi
{
    public static IEndpointRouteBuilder MapCustomersAPI (this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("/api/v1")
               .MapCustomersAPI()
               .WithTags("Customer Api");

        return builder;
    }
    public static RouteGroupBuilder MapCustomersAPI(this RouteGroupBuilder group)
    {
        group.MapGet("customers", GetCustomersAsync);
        group.MapGet("customers/{customerName}", GetCustomerByUserNameAsync);
        group.MapPost("customers", CreateCustomerAsync);
        group.MapDelete("customers/{id}", DeleteCustomerAsync);
        group.MapPut("customers/{id}", UpdateCustomerAsync);

        return group;
    }

    private static async Task<IResult> GetCustomersAsync(ICustomerService customerService)
    {
        return await customerService.GetCustomersAsync();
    }

    private static async Task<IResult> GetCustomerByUserNameAsync( string customerName, ICustomerService customerService)
    {
        return await customerService.GetCustomerByUserNameAsync(customerName);
    }

    private static async Task<IResult> CreateCustomerAsync(CreateCustomerDto newCustomer, ICustomerService customerService)
    {
        return await customerService.CreateCustomerAsync(newCustomer);
    }

    private static async Task<IResult> DeleteCustomerAsync (int id, ICustomerService customerService)
    {
        return await customerService.DeleteCustomerAsync(id);
    }

    private static async Task<IResult> UpdateCustomerAsync(
        [FromQuery]int id, [FromBody] UpdateCustomerDto updateDto,
        ICustomerService customerService)
    {
        return await customerService.UpdateCustomerAsync(id, updateDto);
    }
}
