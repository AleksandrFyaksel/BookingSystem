using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.DAL.Repositories
{
    public class EfUserPasswordRepository : IRepository<UserPassword>
    {
        private readonly BookingContext context;
        private readonly DbSet<UserPassword> userPasswords;

        public EfUserPasswordRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            userPasswords = context.UserPasswords;
        }

        public async Task AddAsync(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await userPasswords.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userPassword = await userPasswords.FindAsync(id);
            if (userPassword == null) return false;
            userPasswords.Remove(userPassword);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<UserPassword> Find(Expression<Func<UserPassword, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return userPasswords.Where(predicate);
        }

        public async Task<IEnumerable<UserPassword>> FindAsync(Expression<Func<UserPassword, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await userPasswords.Where(predicate).ToListAsync();
        }

        public async Task<UserPassword> GetAsync(int id, params string[] includes)
        {
            IQueryable<UserPassword> query = userPasswords;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(up => up.UserPasswordID == id);
        }

        public UserPassword Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<UserPassword> GetAll()
        {
            return userPasswords.AsQueryable();
        }

        public IQueryable<UserPassword> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return userPasswords.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<UserPassword> GetAll(Expression<Func<UserPassword, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? userPasswords.OrderBy(orderBy) : userPasswords.OrderByDescending(orderBy);
        }

        public async Task UpdateAsync(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            userPasswords.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(UserPassword entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(UserPassword entity) => UpdateAsync(entity).GetAwaiter().GetResult();

        public async Task<UserPassword> FirstOrDefaultAsync(Expression<Func<UserPassword, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await userPasswords.FirstOrDefaultAsync(predicate);
        }

        // Реализация метода Remove
        public void Remove(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            userPasswords.Remove(entity);
            context.SaveChanges();
        }

        public async Task<bool> RemoveAsync(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Remove(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public IQueryable<UserPassword> GetAll(Expression<Func<UserPassword, bool>> filter, Expression<Func<UserPassword, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));

            var query = userPasswords.Where(filter);
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}