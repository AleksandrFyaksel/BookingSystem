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
    public class EfBookingStatusRepository : IRepository<BookingStatus>
    {
        private readonly BookingContext context;

        public EfBookingStatusRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            context.BookingStatuses.Add(entity);
            context.SaveChanges();
        }

        public async Task AddAsync(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await context.BookingStatuses.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public bool Delete(int id)
        {
            var bookingStatus = context.BookingStatuses.Find(id);
            if (bookingStatus == null) return false;
            context.BookingStatuses.Remove(bookingStatus);
            context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bookingStatus = await context.BookingStatuses.FindAsync(id);
            if (bookingStatus == null) return false;
            context.BookingStatuses.Remove(bookingStatus);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<BookingStatus> Find(Expression<Func<BookingStatus, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return context.BookingStatuses.Where(predicate);
        }

        public BookingStatus Get(int id, params string[] includes)
        {
            IQueryable<BookingStatus> query = context.BookingStatuses;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(b => b.BookingStatusID == id);
        }

        public async Task<BookingStatus> GetAsync(int id, params string[] includes)
        {
            IQueryable<BookingStatus> query = context.BookingStatuses;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(b => b.BookingStatusID == id);
        }

        public IQueryable<BookingStatus> GetAll()
        {
            return context.BookingStatuses.AsQueryable();
        }

        public IQueryable<BookingStatus> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return context.BookingStatuses.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<BookingStatus> GetAll(Expression<Func<BookingStatus, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? context.BookingStatuses.OrderBy(orderBy) : context.BookingStatuses.OrderByDescending(orderBy);
        }

        public async Task UpdateAsync(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            context.BookingStatuses.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Update(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            context.BookingStatuses.Update(entity);
            context.SaveChanges();
        }

        public async Task<IEnumerable<BookingStatus>> FindAsync(Expression<Func<BookingStatus, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await context.BookingStatuses.Where(predicate).ToListAsync();
        }

        public IQueryable<BookingStatus> GetAll(Expression<Func<BookingStatus, bool>> filter, Expression<Func<BookingStatus, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            var query = context.BookingStatuses.Where(filter);

            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}