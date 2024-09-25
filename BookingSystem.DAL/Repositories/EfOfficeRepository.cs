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

        public async Task UpdateAsync(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            offices.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(Office entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(Office entity) => UpdateAsync(entity).GetAwaiter().GetResult();

        public async Task<Office> FirstOrDefaultAsync(Expression<Func<Office, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await offices.FirstOrDefaultAsync(predicate);
        }

        // Реализация метода GetAll с фильтром, сортировкой и пагинацией
        public IQueryable<Office> GetAll(Expression<Func<Office, bool>> filter, Expression<Func<Office, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));

            var query = offices.Where(filter);
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

       
        public void Remove(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            offices.Remove(entity);
            context.SaveChanges();
        }

        public IQueryable<Office> GetAll(Expression<Func<Office, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? offices.OrderBy(orderBy) : offices.OrderByDescending(orderBy);
        }


        public async Task<bool> RemoveAsync(Office entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Remove(entity);
            return await context.SaveChangesAsync() > 0;
        }
    }
}