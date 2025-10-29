using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APIService.Repository.Implementations
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ShopDbContext _context;

        public RepositoryBase(ShopDbContext context)
        {
            _context = context;
        }

        public async Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        //public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges, Func<IQueryable<T>, IQueryable<T>> include = null)
        //{
        //    var query = _context.Set<T>().Where(expression);
        //    query = !trackChanges ? query.AsNoTracking() : query;
        //    return include != null ? include(query) : query;
        //}

        public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges, bool includeRole = false)
        {
            var query = _context.Set<T>().Where(expression);

            if (includeRole && typeof(T) == typeof(Accounts))
            {
                query = query.Include("Role");
            }

            return !trackChanges ? query.AsNoTracking() : query;
        }

        public IQueryable<T> FindByConditionQuery(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            var query = _context.Set<T>().Where(expression);
            return !trackChanges ? query.AsNoTracking() : query;
        }

        //public async Task<T?> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges, Func<IQueryable<T>, IQueryable<T>> include = null)
        //{
        //    var query = _context.Set<T>().Where(expression);
        //    query = !trackChanges ? query.AsNoTracking() : query;
        //    if(include != null)
        //    {
        //        query = include(query);
        //    }
        //    return await query.FirstOrDefaultAsync();
        //}

        public async Task<T?> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges, bool includeRole = false)
        {
            var query = _context.Set<T>().Where(expression);

            if (includeRole)
            {
                query = query.Include("Role");
            }

            return !trackChanges
                ? await query.AsNoTracking().FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }

        public IQueryable<T> FindAll(bool trackChanges, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            var query = !trackChanges ? _context.Set<T>().AsNoTracking() : _context.Set<T>();
            if(include != null)
            {
                query = include(query);
            }
            return query;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
