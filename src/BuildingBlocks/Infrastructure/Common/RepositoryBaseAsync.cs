using Contracts.Common;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Infrastructure.Common;

public class RepositoryBaseAsync<TEntity, TKey, TContext> : RepositoryQueryBase<TEntity, TKey, TContext>, 
    IRepositoryBaseAsync<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey>
    where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IUnitOfWork<TContext> _unitOfWork;

    public RepositoryBaseAsync(TContext dbContext, IUnitOfWork<TContext> unitOfWork) : base (dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
    }

    #region CRUD Operations
    public async Task<TKey> CreateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
        return await Task.FromResult(entity.Id);
    }

    public async Task<IList<TKey>> CreateListAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.AddRange(entities);
        var ids = entities.Select(e => e.Id).ToList();
        return await Task.FromResult(ids);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged) return;

        TEntity? exist = await _dbContext.Set<TEntity>().FindAsync(entity.Id);
        if(exist != null)
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
    }

    public Task UpdateListAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }
    #endregion

    #region Transaction Management
    public async Task<IDbContextTransaction> BeginTransactionAsync() => await _dbContext.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        await SaveChangeAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task SaveChangeAsync() => await _unitOfWork.CommitAsync();

    public async Task RollbackTransactionAsync() => await _dbContext.Database.RollbackTransactionAsync();
    #endregion

}