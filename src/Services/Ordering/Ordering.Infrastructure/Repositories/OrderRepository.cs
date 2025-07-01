using Contracts.Common;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBaseAsync<OrderEntity, long, OrderContext>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) : base(dbContext, unitOfWork)
    {
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByUserName(string userName)
    {
        var result = await FindByCondition(o => o.UserName.Equals(userName)).ToListAsync();
        return result;
    }
}
