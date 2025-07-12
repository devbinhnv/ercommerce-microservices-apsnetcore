using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces;

public interface ICustomerRepository : IRepositoryBaseAsync<CustomerEntity, int, CustomerContext>
{
    Task<CustomerEntity?> GetCustomerByUserNameAsync(string userName);
    Task<IEnumerable<CustomerEntity>> GetCustomers();
}
