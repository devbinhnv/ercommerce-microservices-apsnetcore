using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace Contracts.Common;

public interface IRepositoryBaseAsync<TEntity, TKey, TContext> : IRepositoryQueryBase<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey>
    where TContext : DbContext
{
    Task<TKey> CreateAsync(TEntity entity);

    Task<IList<TKey>> CreateListAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);

    Task UpdateListAsync(IEnumerable<TEntity> entities);

    Task DeleteAsync(TEntity entity);

    Task DeleteListAsync(IEnumerable<TEntity> entities);

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EndTransactionAsync();

    Task RollbackTransactionAsync();

    Task SaveChangeAsync();
}

public interface IRepositoryBaseAsync<TEntity, TKey> : IRepositoryQueryBase<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    Task<TKey> CreateAsync(TEntity entity);

    Task<IList<TKey>> CreateListAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);

    Task UpdateListAsync(IEnumerable<TEntity> entities);

    Task DeleteAsync(TEntity entity);

    Task DeleteListAsync(IEnumerable<TEntity> entities);

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EndTransactionAsync();

    Task RollbackTransactionAsync();

    Task SaveChangeAsync();
}