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
    public class EfParkingSpaceRepository : IRepository<ParkingSpace>
    {
        private readonly BookingContext context;
        private readonly DbSet<ParkingSpace> parkingSpaces;

        public EfParkingSpaceRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            parkingSpaces = context.ParkingSpaces;
        }

        public async Task AddAsync(ParkingSpace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await parkingSpaces.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parkingSpace = await parkingSpaces.FindAsync(id);
            if (parkingSpace == null) return false;
            parkingSpaces.Remove(parkingSpace);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<ParkingSpace> Find(Expression<Func<ParkingSpace, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return parkingSpaces.Where(predicate);
        }

        public async Task<IEnumerable<ParkingSpace>> FindAsync(Expression<Func<ParkingSpace, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await parkingSpaces.Where(predicate).ToListAsync();
        }

        public async Task<ParkingSpace> GetAsync(int id, params string[] includes)
        {
            IQueryable<ParkingSpace> query = parkingSpaces;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(ps => ps.ParkingSpaceID == id);
        }

        public ParkingSpace Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<ParkingSpace> GetAll()
        {
            return parkingSpaces.AsQueryable();
        }

        public IQueryable<ParkingSpace> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return parkingSpaces.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<ParkingSpace> GetAll(Func<ParkingSpace, object> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? parkingSpaces.OrderBy(orderBy).AsQueryable() : parkingSpaces.OrderByDescending(orderBy).AsQueryable();
        }

        // Добавленный метод для реализации интерфейса
        public IQueryable<ParkingSpace> GetAll(Expression<Func<ParkingSpace, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? parkingSpaces.OrderBy(orderBy) : parkingSpaces.OrderByDescending(orderBy);
        }

        public async Task UpdateAsync(ParkingSpace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            parkingSpaces.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(ParkingSpace entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(ParkingSpace entity) => UpdateAsync(entity).GetAwaiter().GetResult();

        public IQueryable<ParkingSpace> GetAll(Expression<Func<ParkingSpace, bool>> filter, Expression<Func<ParkingSpace, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}