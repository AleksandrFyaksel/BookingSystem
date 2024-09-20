using BookingSystem.DAL.Data;
using BookingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BookingContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(BookingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(int pageNumber, int pageSize)
        {
            return _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>> orderBy, bool ascending = true)
        {
            return ascending ? _dbSet.OrderBy(orderBy) : _dbSet.OrderByDescending(orderBy);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            var query = _dbSet.Where(filter);
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public T Get(int id, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(int id, params string[] includes)
        {
            throw new NotImplementedException();
        }
    }
}