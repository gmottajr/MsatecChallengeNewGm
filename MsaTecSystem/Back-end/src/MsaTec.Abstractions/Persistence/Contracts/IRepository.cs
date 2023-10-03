using MsaTec.Abstractions.Model.Contracts;
using System.Linq.Expressions;

namespace MsaTec.Abstractions.Persistence.Contracts;

public interface IRepository<T> where T : IEntityRoot
{
    Task<T?> QuerySingleAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    Task InsertAsync(T entity);
    Task Update(T entity);
    Task DeleteAsync(Guid id);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    Task<bool> SaveChangesAsync();
}
