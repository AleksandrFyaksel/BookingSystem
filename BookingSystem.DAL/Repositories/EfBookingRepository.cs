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
    public class EfBookingRepository : IRepository<Booking>
    {
        private readonly BookingContext context;
        private readonly DbSet<Booking> bookings;

        public EfBookingRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            bookings = context.Bookings;
        }

        public async Task AddAsync(Booking entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await bookings.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await bookings.FindAsync(id);
            if (booking == null) return false;
            bookings.Remove(booking);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Booking> Find(Expression<Func<Booking, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return bookings.Where(predicate);
        }

        public async Task<IEnumerable<Booking>> FindAsync(Expression<Func<Booking, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await bookings.Where(predicate).ToListAsync();
        }

        public async Task<Booking> GetAsync(int id, params string[] includes)
        {
            IQueryable<Booking> query = bookings;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(b => b.BookingID == id);
        }

        public Booking Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<Booking> GetAll()
        {
            return bookings.AsQueryable();
        }

        public IQueryable<Booking> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return bookings.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Booking> GetAll(Func<Booking, object> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? bookings.OrderBy(orderBy).AsQueryable() : bookings.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Booking> GetAll(Expression<Func<Booking, object>> include, bool asNoTracking)
        {
            IQueryable<Booking> query = bookings;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query.Include(include);
        }

        public async Task UpdateAsync(Booking entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bookings.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(Booking entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(Booking entity) => UpdateAsync(entity).GetAwaiter().GetResult();
    }
}