using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services;

public class CustomerService (ICustomerRepository customerRepository): ICustomerService
{
    public async Task<IResult> GetCustomerByUserNameAsync(string userName)
    {
        var customer = await customerRepository.GetCustomerByUserNameAsync(userName);
        return customer != null ? Results.Ok(customer) : Results.NotFound();
    }

    public async Task<IResult> GetCustomersAsync() => Results.Ok(await customerRepository.GetCustomers());
}
