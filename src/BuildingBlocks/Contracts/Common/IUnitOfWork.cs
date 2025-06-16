using Microsoft.EntityFrameworkCore;

namespace Contracts.Common;

public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
{
    Task<int> CommitAsync();
}
