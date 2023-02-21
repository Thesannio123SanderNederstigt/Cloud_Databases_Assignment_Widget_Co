using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IRepository<TEntity, TId> where TEntity : class
{
    public Task<TEntity?> GetByIdAsync(TId id);
    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter);
    public IAsyncEnumerable<TEntity> GetAllAsync();
    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    public Task InsertAsync(TEntity entity);
    public void Remove(TEntity entity);
    public Task SaveChanges();

    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, TProp>> property);
    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, IEnumerable<TProp>>> property);
    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, ICollection<TProp>>> property);
}
