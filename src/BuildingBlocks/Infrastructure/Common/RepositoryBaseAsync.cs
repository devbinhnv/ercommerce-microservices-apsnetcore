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
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    #region Query Operations
    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        return trackChanges
            ? _context.Set<TEntity>()
            : _context.Set<TEntity>().AsNoTracking();
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        return includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return trackChanges
            ? _context.Set<TEntity>().Where(expression)
            : _context.Set<TEntity>().Where(expression).AsNoTracking();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        return includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        return await FindByCondition(e => e.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        return await FindByCondition(e => e.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();
    }
    #endregion

    #region CRUD Operations
    public async Task<TKey> CreateAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        await _context.Set<TEntity>().AddAsync(entity);
        return entity.Id;
    }

    public async Task<IList<TKey>> CreateListAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        var entityList = entities.ToList();
        if (!entityList.Any()) return new List<TKey>();

        await _context.AddRangeAsync(entityList);
        return entityList.Select(e => e.Id).ToList();
    }

    public Task UpdateAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var tracked = _context.Entry(entity);
        if (tracked.State == EntityState.Unchanged) return Task.CompletedTask;

        if (tracked.State == EntityState.Detached)
        {
            var existing = _context.Set<TEntity>().Find(entity.Id);
            if (existing == null)
                throw new InvalidOperationException($"Entity with Id {entity.Id} not found");

            _context.Entry(existing).CurrentValues.SetValues(entity);
        }

        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        var entityList = entities.ToList();
        if (!entityList.Any()) return Task.CompletedTask;

        _context.Set<TEntity>().UpdateRange(entityList);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        var entityList = entities.ToList();
        if (!entityList.Any()) return Task.CompletedTask;

        _context.Set<TEntity>().RemoveRange(entityList);
        return Task.CompletedTask;
    }
    #endregion

    #region Transaction Management
    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        try
        {
            await SaveChangeAsync();
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CommitTransactionAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public async Task SaveChangeAsync() => await _unitOfWork.CommitAsync();

    public async Task RollbackTransactionAsync()
    {
        if (_context.Database.CurrentTransaction != null)
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
    #endregion
}