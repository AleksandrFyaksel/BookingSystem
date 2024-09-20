using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class OfficeTestRepository : IRepository<Office>
    {
        private readonly List<Office> offices;

        public OfficeTestRepository(List<Office> offices)
        {
            this.offices = offices ?? throw new ArgumentNullException(nameof(offices));
            SetupData();
        }

        private void SetupData()
        {
            if (!offices.Any())
            {
                offices.Add(new Office { OfficeID = 1, OfficeName = "Офис 1", Location = "Минск", Capacity = 10, Phone = "123456789" });
                offices.Add(new Office { OfficeID = 2, OfficeName = "Офис 2", Location = "Витебск", Capacity = 20, Phone = "987654321" });
            }
        }

        public void Add(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            offices.Add(entity);
        }

        public async Task AddAsync(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            offices.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var office = Get(id);
            if (office == null) return false;
            return offices.Remove(office);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var office = Get(id);
            if (office == null) return false;
            offices.Remove(office);
            return await Task.FromResult(true);
        }

        public IQueryable<Office> Find(Expression<Func<Office, bool>> predicate) => offices.AsQueryable().Where(predicate);

        public Office Get(int id, params string[] includes) => offices.FirstOrDefault(o => o.OfficeID == id);

        public async Task<Office> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<Office> GetAll() => offices.AsQueryable();

        public IQueryable<Office> GetAll(Func<Office, object> orderBy, bool ascending = true)
        {
            return ascending ? offices.OrderBy(orderBy).AsQueryable() : offices.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Office> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return offices.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<Office> GetAll(Expression<Func<Office, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return offices.AsQueryable();
        }

        public void Update(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existingOffice = Get(entity.OfficeID);
            if (existingOffice != null)
            {
                existingOffice.OfficeName = entity.OfficeName;
                existingOffice.Location = entity.Location;
                existingOffice.Capacity = entity.Capacity;
                existingOffice.Phone = entity.Phone;
            }
        }

        public async Task UpdateAsync(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Office>> FindAsync(Expression<Func<Office, bool>> predicate)
        {
            return await Task.FromResult(offices.AsQueryable().Where(predicate).ToList());
        }

        public IQueryable<Office> GetAll(Expression<Func<Office, bool>> filter, Expression<Func<Office, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}