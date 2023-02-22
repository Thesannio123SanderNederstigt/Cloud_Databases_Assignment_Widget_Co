using System.Linq.Expressions;

namespace Repository.Interfaces;
public interface IIncludableRepository<TEntity, TProp>
{
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property);
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, TNew>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, IEnumerable<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, ICollection<TNew>>> property);

    public Task<TEntity?> GetBy(Expression<Func<TEntity, bool>> filter);
    public IAsyncEnumerable<TEntity> GetAll();
    public IAsyncEnumerable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> filter);
    public Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
}