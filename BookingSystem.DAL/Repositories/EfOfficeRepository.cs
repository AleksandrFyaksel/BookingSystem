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
    public class EfOfficeRepository : IRepository<Office>
    {
        private readonly BookingContext context;
        private readonly DbSet<Office> offices;

        public EfOfficeRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            offices = context.Offices;
        }

        public async Task AddAsync(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await offices.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var office = await offices.FindAsync(id);
            if (office == null) return false;
            offices.Remove(office);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Office> Find(Expression<Func<Office, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return offices.Where(predicate);
        }

        public async Task<IEnumerable<Office>> FindAsync(Expression<Func<Office, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await offices.Where(predicate).ToListAsync();
        }

        public async Task<Office> GetAsync(int id, params string[] includes)
        {
            IQueryable<Office> query = offices;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(o => o.OfficeID == id);
        }

        public Office Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<Office> GetAll()
        {
            return offices.AsQueryable();
        }

        public IQueryable<Office> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return offices.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Office> GetAll(Func<Office, object> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? offices.OrderBy(orderBy).AsQueryable() : offices.OrderByDescending(orderBy).AsQueryable();
        }

        // Добавленный метод для реализации интерфейса
        public IQueryable<Office> GetAll(Expression<Func<Office, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? offices.OrderBy(orderBy) : offices.OrderByDescending(orderBy);
        }

        public async Task UpdateAsync(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            offices.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(Office entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(Office entity) => UpdateAsync(entity).GetAwaiter().GetResult();
    }
}