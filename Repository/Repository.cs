using System.Linq.Expressions;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository;

public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
{
    private readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbset;

    protected Repository(DbContext context, DbSet<TEntity> dbset)
    {
        _context = context;
        _dbset = dbset;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _dbset.FindAsync(id);
    }

    public async Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbset.FirstOrDefaultAsync(filter);
    }

    public IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return _dbset.AsAsyncEnumerable();
    }

    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter)
    {
        return _dbset.Where(filter).AsAsyncEnumerable();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbset.AnyAsync(predicate);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _dbset.AddAsync(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbset.Remove(entity);
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}