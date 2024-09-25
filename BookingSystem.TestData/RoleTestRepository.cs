using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class RoleTestRepository : IRepository<Role>
    {
        private readonly List<Role> roles;

        public RoleTestRepository(List<Role> roles)
        {
            this.roles = roles ?? throw new ArgumentNullException(nameof(roles));
            SetupData();
        }

        private void SetupData()
        {
            if (!roles.Any())
            {
                roles.Add(new Role { RoleID = 1, RoleName = "Роль 1" });
                roles.Add(new Role { RoleID = 2, RoleName = "Роль 2" });
            }
        }

        public void Add(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            roles.Add(entity);
        }

        public async Task AddAsync(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            roles.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var role = Get(id);
            return role != null && roles.Remove(role);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = Get(id);
            if (role == null) return false;
            roles.Remove(role);
            return await Task.FromResult(true);
        }

        public IQueryable<Role> Find(Expression<Func<Role, bool>> predicate) => roles.AsQueryable().Where(predicate);

        public Role Get(int id, params string[] includes) => roles.FirstOrDefault(r => r.RoleID == id);

        public async Task<Role> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<Role> GetAll() => roles.AsQueryable();

        public IQueryable<Role> GetAll(Func<Role, object> orderBy, bool ascending = true)
        {
            return ascending ? roles.OrderBy(orderBy).AsQueryable() : roles.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Role> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return roles.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<Role> GetAll(Expression<Func<Role, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return roles.AsQueryable();
        }

        public void Update(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existingRole = Get(entity.RoleID);
            if (existingRole != null)
            {
                existingRole.RoleName = entity.RoleName;
            }
        }

        public async Task UpdateAsync(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate)
        {
            return await Task.FromResult(roles.AsQueryable().Where(predicate).ToList());
        }

        public IQueryable<Role> GetAll(Expression<Func<Role, bool>> filter, Expression<Func<Role, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<Role> FirstOrDefaultAsync(Expression<Func<Role, bool>> predicate)
        {
            return Task.FromResult(roles.AsQueryable().FirstOrDefault(predicate));
        }

        // Реализация метода Remove
        public void Remove(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            roles.Remove(entity);
        }

        public Task<bool> RemoveAsync(Role entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Remove(entity);
            return Task.FromResult(true);
        }
    }
}