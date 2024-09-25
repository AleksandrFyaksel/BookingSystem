using System.Linq.Expressions;
using System.Threading.Tasks;
using BookingSystem.DAL.Data;
using BookingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.DAL.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly BookingContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BookingContext context)
        {
            _context = context;
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
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T Get(int id, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id); 
        }

        public async Task<T> GetAsync(int id, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id); // Предполагается, что Id - это свойство
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
            await Task.CompletedTask; 
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        // Реализация метода Remove
        public void Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}