using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.Domain.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        Task AddAsync(T entity);
        bool Delete(int id);
        Task<bool> DeleteAsync(int id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        T Get(int id, params string[] includes);
        Task<T> GetAsync(int id, params string[] includes);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(int pageNumber, int pageSize);
        IQueryable<T> GetAll(Expression<Func<T, object>> orderBy, bool ascending = true);
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10);
        void Update(T entity);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}