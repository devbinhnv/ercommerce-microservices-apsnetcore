using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces;

public interface IOrderRepository : IRepositoryBaseAsync<OrderEntity, long>
{
    Task<IEnumerable<OrderEntity>> GetOrdersByUserName(string userName);
}
