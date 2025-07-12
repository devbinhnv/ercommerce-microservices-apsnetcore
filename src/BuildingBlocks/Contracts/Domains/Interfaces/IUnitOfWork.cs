using Microsoft.EntityFrameworkCore;

namespace Contracts.Domains.Interfaces;

public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
{
    Task<int> CommitAsync();
}
