using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Taskify.Domain.Common.Classes;
using Taskify.Domain.Repositories;
using Taskify.Infrastructure.Contexts;

namespace Taskify.Infrastructure.Repositories
{
    public class BaseRepository<T, TKEY>(ApplicationDbContext context) : IBaseRepository<T, TKEY> where T : BaseEntity<TKEY>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(T item) => await _context.Set<T>().AddAsync(item);

        public async Task<bool> AnyAsync() => await _context.Set<T>().AnyAsync();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().AnyAsync(predicate);

        public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
             => await _context.Set<T>().CountAsync(predicate);
        public void Delete(T item) => _context.Set<T>().Remove(item);

        public IQueryable<T> GetAll() => _context.Set<T>();

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);

        public async Task<T?> GetByIdAsync(TKEY id) => await _context.Set<T>().FindAsync(id);

        public void Update(T item) => _context.Set<T>().Update(item);
    }
}
