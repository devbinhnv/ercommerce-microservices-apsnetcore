using Contracts.Common;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Infrastructure.Common;

public class RepositoryBaseAsync<TEntity, TKey, TContext> : IRepositoryBaseAsync<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey>
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IUnitOfWork<TContext> _unitOfWork;

    public RepositoryBaseAsync(TContext context, IUnitOfWork<TContext> unitOfWork)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
    }

    #region Query Operations
    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        if (trackChanges)
        {
            return _context.Set<TEntity>();
        }
        return _context.Set<TEntity>().AsNoTracking();
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        if (trackChanges)
        {
            return _context.Set<TEntity>().Where(expression);
        }
        return _context.Set<TEntity>().Where(expression).AsNoTracking();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await FindByCondition(e => e.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindByCondition(e => e.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();
    }
    #endregion

    #region CRUD Operations
    public async Task<TKey> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        return entity.Id;
    }

    public async Task<IList<TKey>> CreateListAsync(IEnumerable<TEntity> entities)
    {
        await _context.AddRangeAsync(entities);
        return entities.Select(e => e.Id).ToList();
    }

    public Task UpdateAsync(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Unchanged) return Task.CompletedTask;

        TEntity exist = _context.Set<TEntity>().Find(entity.Id);
        _context.Entry(exist).CurrentValues.SetValues(entity);

        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }
    #endregion

    #region Transaction Management
    public async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        await SaveChangeAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public async Task SaveChangeAsync() => await _unitOfWork.CommitAsync();

    public async Task RollbackTransactionAsync() => await _context.Database.RollbackTransactionAsync();
    #endregion

}