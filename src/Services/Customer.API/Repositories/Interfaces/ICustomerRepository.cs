using Contracts.Common;
using Customer.API.Entities;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces;

public interface ICustomerRepository : IRepositoryBaseAsync<Entities.Customer, int, CustomerContext>
{
    Task<Entities.Customer?> GetCustomerByUserNameAsync(string userName);
    Task<IEnumerable<Entities.Customer>> GetCustomers();
}
