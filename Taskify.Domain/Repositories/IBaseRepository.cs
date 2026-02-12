using System.Linq.Expressions;

namespace Taskify.Domain.Repositories
{
    public interface IBaseRepository<T, TKEY> where T : class
    {
        Task<T?> GetByIdAsync(TKEY id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}