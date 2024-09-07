using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class UserTestRepository : IRepository<User>
    {
        private readonly List<User> users;

        public UserTestRepository(List<User> users)
        {
            this.users = users ?? throw new ArgumentNullException(nameof(users));
            SetupData();
        }

        private void SetupData()
        {
            if (!users.Any())
            {
                // Инициализация тестовых данных
                users.Add(new User { UserID = 1, Name = "user1", Email = "user1@example.com" });
                users.Add(new User { UserID = 2, Name = "user2", Email = "user2@example.com" });
            }
        }

        public void Add(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            users.Add(entity);
        }

        public async Task AddAsync(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            users.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var user = Get(id);
            return user != null && users.Remove(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = Get(id);
            if (user == null) return false;
            users.Remove(user);
            return await Task.FromResult(true);
        }

        public IQueryable<User> Find(Expression<Func<User, bool>> predicate) => users.AsQueryable().Where(predicate);

        public User Get(int id, params string[] includes) => users.FirstOrDefault(u => u.UserID == id);

        public async Task<User> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<User> GetAll() => users.AsQueryable();

        public IQueryable<User> GetAll(Func<User, object> orderBy, bool ascending = true)
        {
            return ascending ? users.OrderBy(orderBy).AsQueryable() : users.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<User> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return users.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<User> GetAll(Expression<Func<User, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return users.AsQueryable();
        }

        public void Update(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existingUser = Get(entity.UserID);
            if (existingUser != null)
            {
                existingUser.Name = entity.Name;
                existingUser.Email = entity.Email;
                existingUser.PasswordHash = entity.PasswordHash;
                existingUser.PhoneNumber = entity.PhoneNumber;
                existingUser.Role = entity.Role;
                existingUser.UserPasswords = entity.UserPasswords ?? new List<UserPassword>();
            }
        }

        public async Task UpdateAsync(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await Task.FromResult(users.AsQueryable().Where(predicate).ToList());
        }
    }
}