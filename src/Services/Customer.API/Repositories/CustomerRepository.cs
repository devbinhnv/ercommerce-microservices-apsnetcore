using Contracts.Domains.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryBaseAsync<CustomerEntity, int, CustomerContext>,
    ICustomerRepository
{
    public CustomerRepository(CustomerContext context, IUnitOfWork<CustomerContext> unitOfWork) 
        : base(context, unitOfWork)
    {
    }

    public async Task<CustomerEntity?> GetCustomerByUserNameAsync(string userName) => await FindByCondition(x => x.UserName.Equals(userName))
        .SingleOrDefaultAsync();

    public async Task<IEnumerable<CustomerEntity>> GetCustomers() => await FindAll().ToListAsync();
}
