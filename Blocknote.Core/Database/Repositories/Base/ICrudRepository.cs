using System.Linq.Expressions;

namespace Blocknote.Core.Database.Repositories.Base;

public interface ICrudRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AddAsync(TEntity entity);
    Task<bool> EditAsync(TEntity entity);
    Task<bool> DeleteAsync(Guid id);
}