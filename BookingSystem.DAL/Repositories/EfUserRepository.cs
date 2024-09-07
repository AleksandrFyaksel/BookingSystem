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
    public class EfUserRepository : IRepository<User>
    {
        private readonly BookingContext context;
        private readonly DbSet<User> users;

        public EfUserRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            users = context.Users;
        }

        public async Task AddAsync(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await users.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Add(User entity) => AddAsync(entity).GetAwaiter().GetResult();

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await users.FindAsync(id);
            if (user == null) return false;
            users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();

        public IQueryable<User> Find(Expression<Func<User, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return users.Where(predicate);
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await users.Where(predicate).ToListAsync();
        }

        public async Task<User> GetAsync(int id, params string[] includes)
        {
            IQueryable<User> query = users;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(u => u.UserID == id);
        }

        public User Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<User> GetAll()
        {
            return users.AsQueryable();
        }

        public IQueryable<User> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return users.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        // Добавленный метод для реализации интерфейса
        public IQueryable<User> GetAll(Expression<Func<User, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? users.OrderBy(orderBy) : users.OrderByDescending(orderBy);
        }

        public async Task UpdateAsync(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            users.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Update(User entity) => UpdateAsync(entity).GetAwaiter().GetResult();
    }
}