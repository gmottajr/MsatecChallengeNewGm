using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MsaTec.Abstractions.Model.Base;
using MsaTec.DAL.Repositories.Contracts;
using System.Linq.Expressions;

namespace MsaTec.DAL.Repositories.Base;

public abstract class RepositoryBase<TEntity> : IDataRepository<TEntity> where TEntity : EntityRootBase
{
    protected DbSet<TEntity> _DbSet;
    protected readonly DbContext _DbContext;

    protected RepositoryBase(DbContext dbContext)
    {
        _DbContext = dbContext;
    }

    public virtual IQueryable<TEntity> Query => _DbSet.AsQueryable();

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _DbSet.FindAsync(id);
        
        if (entity != null)
          _DbSet.Remove(entity);

        await SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        if (orderBy == null && include == null)
            return await _DbSet.ToListAsync();

        IQueryable<TEntity> query = _DbSet;

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _DbSet.FindAsync(id);
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        entity.Validate();
        await _DbSet.AddAsync(entity);
        await this.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>>? expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        if (include == null && orderBy == null)
            return expression != null ? await _DbSet.Where(expression).ToListAsync() : await _DbSet.ToListAsync();

        IQueryable<TEntity> query = _DbSet;

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null && expression != null)
        {
            return await orderBy(query).Where(expression).ToListAsync();
        }

        if (orderBy != null && expression == null)
        {
            return await orderBy(query).ToListAsync();
        }

        return (expression != null) ? await query.Where(expression).AsNoTracking().ToListAsync() : await query.AsNoTracking().ToListAsync();
        
    }

    public virtual async Task<TEntity?> QuerySingleAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        if (include == null && orderBy == null)
            return await _DbSet.FirstOrDefaultAsync(expression);

        IQueryable<TEntity> query = _DbSet;

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            return await orderBy(query).Where(expression).FirstOrDefaultAsync();
        }

        return await query.Where(expression).FirstOrDefaultAsync();
    }

    public abstract Task<bool> SaveChangesAsync();

    public virtual async Task Update(TEntity entity)
    {
        entity.Validate();
        _DbSet.Update(entity);
        await SaveChangesAsync();
    }

    public virtual async Task<TEntity?> QuerySingleAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        return await QuerySingleAsync(expression, orderBy, null);
    }

    public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        return await QueryAsync(expression, orderBy, null);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        return await GetAllAsync(orderBy, null);
    }
}
