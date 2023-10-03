using Microsoft.EntityFrameworkCore.Query;
using MsaTec.Abstractions.Model.Contracts;
using MsaTec.Abstractions.Persistence.Contracts;
using MsaTec.DAL.Data.Contracts;
using System.Linq.Expressions;

namespace MsaTec.DAL.Repositories.Contracts;

public interface IDataRepository<T> : IRepository<T> where T : IEntityRoot
{
    /// <summary>
    /// var example = this.repository.Get<Entity>( x => x.Id == 1, r => r.Include(c => c.Child).ThenInclude(g => g.Grandson)).FirstOrDefault();
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="OrderByDesc"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    Task<T?> QuerySingleAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
  
    Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
}
