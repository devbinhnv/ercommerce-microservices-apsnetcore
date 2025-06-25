using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customer;

namespace Customer.API.Controllers;

public static class CustomerApi
{
    public static void MapCustomersAPI(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/customers", GetCustomersAsync);
        app.MapGet("/api/customers/{customerName}", GetCustomerByUserNameAsync);
        app.MapPost("/api/customers", CreateCustomerAsync);
        app.MapDelete("/api/customers/{id}", DeleteCustomerAsync);
        app.MapPut("/api/customers/{id}", UpdateCustomerAsync);
    }

    private static async Task<IResult> GetCustomersAsync(ICustomerService customerService)
        => await customerService.GetCustomersAsync();

    private static async Task<IResult> GetCustomerByUserNameAsync( string customerName, ICustomerService customerService)
        => await customerService.GetCustomerByUserNameAsync(customerName);

    private static async Task<IResult> CreateCustomerAsync(CreateCustomerDto newCustomer, ICustomerService customerService)
        => await customerService.CreateCustomerAsync(newCustomer);

    private static async Task<IResult> DeleteCustomerAsync (int id, ICustomerService customerService)
        => await customerService.DeleteCustomerAsync(id);

    private static async Task<IResult> UpdateCustomerAsync(
        [FromQuery]int id, [FromBody] UpdateCustomerDto updateDto,
        ICustomerService customerService)
        => await customerService.UpdateCustomerAsync(id, updateDto);
}
