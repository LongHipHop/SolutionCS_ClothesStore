using System.Linq.Expressions;

namespace APIService.Repository.Interface
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges, Func<IQueryable<T>, IQueryable<T>> include = null);
        IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges, bool includeRole = false);
        Task<T?> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges, bool includeRole = false);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
