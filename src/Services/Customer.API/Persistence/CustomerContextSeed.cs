using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence;

public static class CustomerContextSeed
{
    public static IHost SeedCustomerData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var customerContext = scope.ServiceProvider.GetRequiredService<CustomerContext>();
        customerContext.Database.MigrateAsync().GetAwaiter().GetResult();

        CreateCustomer(customerContext, "customer1", "customer1", "customer", "customer1@local.com").GetAwaiter().GetResult();
        CreateCustomer(customerContext, "customer2", "customer2", "customer", "customer2@local.com").GetAwaiter().GetResult();
        return host;
    }

    private static async Task CreateCustomer(CustomerContext customerContext, string userName, string firstName, string lastName, string email)
    {
        var customer = await customerContext.Customers.FirstOrDefaultAsync(x => x.UserName.Equals(userName) ||
            x.EmailAddress.Equals(email));

        if(customer == null)
        {
            var newCustomer = new Entities.Customer
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email
            };
            await customerContext.Customers.AddAsync(newCustomer);
            await customerContext.SaveChangesAsync();
        }
    }
}
