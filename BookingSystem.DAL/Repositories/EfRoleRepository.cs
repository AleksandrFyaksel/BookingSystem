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
    public class EfRoleRepository : IRepository<Role>
    {
        private readonly BookingContext context;
        private readonly DbSet<Role> roles;

        public EfRoleRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            roles = context.Roles;
        }

        public async Task AddAsync(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await roles.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await roles.FindAsync(id);
            if (role == null) return false;
            roles.Remove(role);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Role> Find(Expression<Func<Role, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return roles.Where(predicate);
        }

        public async Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await roles.Where(predicate).ToListAsync();
        }

        public async Task<Role> GetAsync(int id, params string[] includes)
        {
            IQueryable<Role> query = roles;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(r => r.RoleID == id);
        }

        public Role Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<Role> GetAll()
        {
            return roles.AsQueryable();
        }

        public IQueryable<Role> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return roles.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Role> GetAll(Func<Role, object> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? roles.OrderBy(orderBy).AsQueryable() : roles.OrderByDescending(orderBy).AsQueryable();
        }

        // Добавленный метод для реализации интерфейса
        public IQueryable<Role> GetAll(Expression<Func<Role, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? roles.OrderBy(orderBy) : roles.OrderByDescending(orderBy);
        }

        public async Task UpdateAsync(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            roles.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(Role entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(Role entity) => UpdateAsync(entity).GetAwaiter().GetResult();

        public IQueryable<Role> GetAll(Expression<Func<Role, bool>> filter, Expression<Func<Role, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}