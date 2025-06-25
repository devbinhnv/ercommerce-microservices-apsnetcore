using Shared.DTOs.Customer;

namespace Customer.API.Services.Interfaces;

public interface ICustomerService
{
    Task<IResult> GetCustomersAsync();
    Task<IResult> GetCustomerByUserNameAsync(string customerName);
    Task<IResult> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<IResult> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);
    Task<IResult> DeleteCustomerAsync(int id);
}
