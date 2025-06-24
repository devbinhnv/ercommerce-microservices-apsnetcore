using Contracts.Common;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>,
    ICustomerRepository
{
    public CustomerRepository(CustomerContext context, IUnitOfWork<CustomerContext> unitOfWork) 
        : base(context, unitOfWork)
    {
    }

    public async Task<Entities.Customer?> GetCustomerByUserNameAsync(string userName) => await FindByCondition(x => x.UserName.Equals(userName))
        .SingleOrDefaultAsync();

    public async Task<IEnumerable<Entities.Customer>> GetCustomers() => await FindAll().ToListAsync();
}
