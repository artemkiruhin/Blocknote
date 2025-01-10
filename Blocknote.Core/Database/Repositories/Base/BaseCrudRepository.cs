using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Blocknote.Core.Database.Repositories.Base;

public abstract class BaseCrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    public BaseCrudRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();
            
            await _dbSet.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            
            await _context.Database.CommitTransactionAsync();
            
            return result > 0;
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<bool> EditAsync(TEntity entity)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();
            
            _dbSet.Update(entity);
            var result = await _context.SaveChangesAsync();
            
            await _context.Database.CommitTransactionAsync();
            
            return result > 0;
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            
            await _context.Database.BeginTransactionAsync();
            _dbSet.Remove(entity);
            var result = await _context.SaveChangesAsync();
            
            await _context.Database.CommitTransactionAsync();
            
            return result > 0;
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            return false;
        }
    }
}