using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.Linq.Expressions;

namespace Contracts.Common;

public interface IRepositoryQueryBase<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey>
    where TContext : DbContext
{
    IQueryable<TEntity> FindAll(bool trackChanges = false);

    IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false);

    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity?> GetByIdAsync(TKey id);

    Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);
}

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