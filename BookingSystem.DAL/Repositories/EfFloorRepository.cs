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
    public class EfFloorRepository : IRepository<Floor>
    {
        private readonly BookingContext context;
        private readonly DbSet<Floor> floors;

        public EfFloorRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            floors = context.Floors;
        }

        public async Task AddAsync(Floor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await floors.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var floor = await floors.FindAsync(id);
            if (floor == null) return false;
            floors.Remove(floor);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Floor> Find(Expression<Func<Floor, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return floors.Where(predicate);
        }

        public async Task<IEnumerable<Floor>> FindAsync(Expression<Func<Floor, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await floors.Where(predicate).ToListAsync();
        }

        public async Task<Floor> GetAsync(int id, params string[] includes)
        {
            IQueryable<Floor> query = floors;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(f => f.FloorID == id);
        }

        public Floor Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<Floor> GetAll()
        {
            return floors.AsQueryable();
        }

        public IQueryable<Floor> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return floors.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Floor> GetAll(Func<Floor, object> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? floors.OrderBy(orderBy).AsQueryable() : floors.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Floor> GetAll(Expression<Func<Floor, object>> include, bool asNoTracking)
        {
            IQueryable<Floor> query = floors;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query.Include(include);
        }

        public async Task UpdateAsync(Floor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            floors.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(Floor entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(Floor entity) => UpdateAsync(entity).GetAwaiter().GetResult();

        public IQueryable<Floor> GetAll(Expression<Func<Floor, bool>> filter, Expression<Func<Floor, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}