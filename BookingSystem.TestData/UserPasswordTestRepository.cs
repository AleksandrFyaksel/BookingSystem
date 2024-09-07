using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class UserPasswordTestRepository : IRepository<UserPassword>
    {
        private readonly List<UserPassword> userPasswords;

        public UserPasswordTestRepository(List<UserPassword> userPasswords)
        {
            this.userPasswords = userPasswords ?? throw new ArgumentNullException(nameof(userPasswords));
            SetupData();
        }

        private void SetupData()
        {
            if (!userPasswords.Any())
            {
                userPasswords.Add(new UserPassword { UserPasswordID = 1, PasswordHash = "hash1", Salt = "salt1", UserID = 1 });
                userPasswords.Add(new UserPassword { UserPasswordID = 2, PasswordHash = "hash2", Salt = "salt2", UserID = 2 });
            }
        }

        public void Add(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            userPasswords.Add(entity);
        }

        public async Task AddAsync(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            userPasswords.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var userPassword = Get(id);
            return userPassword != null && userPasswords.Remove(userPassword);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userPassword = Get(id);
            if (userPassword == null) return false;
            userPasswords.Remove(userPassword);
            return await Task.FromResult(true);
        }

        public IQueryable<UserPassword> Find(Expression<Func<UserPassword, bool>> predicate) => userPasswords.AsQueryable().Where(predicate);

        public UserPassword Get(int id, params string[] includes) => userPasswords.FirstOrDefault(up => up.UserPasswordID == id);

        public async Task<UserPassword> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<UserPassword> GetAll() => userPasswords.AsQueryable();

        public IQueryable<UserPassword> GetAll(Func<UserPassword, object> orderBy, bool ascending = true)
        {
            return ascending ? userPasswords.OrderBy(orderBy).AsQueryable() : userPasswords.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<UserPassword> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return userPasswords.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<UserPassword> GetAll(Expression<Func<UserPassword, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return userPasswords.AsQueryable();
        }

        public void Update(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existingUserPassword = Get(entity.UserPasswordID);
            if (existingUserPassword != null)
            {
                existingUserPassword.PasswordHash = entity.PasswordHash;
                existingUserPassword.Salt = entity.Salt;
            }
        }

        public async Task UpdateAsync(UserPassword entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<UserPassword>> FindAsync(Expression<Func<UserPassword, bool>> predicate)
        {
            return await Task.FromResult(userPasswords.AsQueryable().Where(predicate).ToList());
        }
    }
}