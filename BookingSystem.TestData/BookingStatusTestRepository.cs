using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class BookingStatusTestRepository : IRepository<BookingStatus>
    {
        private readonly List<BookingStatus> bookingStatuses;

        public BookingStatusTestRepository(List<BookingStatus> bookingStatuses)
        {
            this.bookingStatuses = bookingStatuses ?? throw new ArgumentNullException(nameof(bookingStatuses));
            SetupData();
        }

        private void SetupData()
        {
            if (!bookingStatuses.Any())
            {
                bookingStatuses.Add(new BookingStatus { BookingStatusID = 1, StatusName = "Активное", Description = "Бронирование активно", IsActive = true });
                bookingStatuses.Add(new BookingStatus { BookingStatusID = 2, StatusName = "Отмененное", Description = "Бронирование отменено", IsActive = false });
                bookingStatuses.Add(new BookingStatus { BookingStatusID = 3, StatusName = "Завершенное", Description = "Бронирование завершено", IsActive = false });
            }
        }

        public void Add(BookingStatus entity) => bookingStatuses.Add(entity);

        public Task AddAsync(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bookingStatuses.Add(entity);
            return Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var bookingStatus = Get(id);
            if (bookingStatus == null) return false;
            return bookingStatuses.Remove(bookingStatus);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bookingStatus = await Task.Run(() => Get(id));
            if (bookingStatus == null) return false;
            bookingStatuses.Remove(bookingStatus);
            return true;
        }

        public IQueryable<BookingStatus> Find(Expression<Func<BookingStatus, bool>> predicate) => bookingStatuses.AsQueryable().Where(predicate);

        public BookingStatus Get(int id, params string[] includes) => bookingStatuses.FirstOrDefault(bs => bs.BookingStatusID == id);

        public Task<BookingStatus> GetAsync(int id, params string[] includes) => Task.FromResult(Get(id, includes));

        public IQueryable<BookingStatus> GetAll() => bookingStatuses.AsQueryable();

        public IQueryable<BookingStatus> GetAll(Expression<Func<BookingStatus, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? bookingStatuses.AsQueryable().OrderBy(orderBy) : bookingStatuses.AsQueryable().OrderByDescending(orderBy);
        }

        public IQueryable<BookingStatus> GetAll(Expression<Func<BookingStatus, bool>> filter, Expression<Func<BookingStatus, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            var query = bookingStatuses.AsQueryable().Where(filter);
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public void Update(BookingStatus entity)
        {
            var existingStatus = Get(entity.BookingStatusID);
            if (existingStatus != null)
            {
                existingStatus.StatusName = entity.StatusName;
                existingStatus.Description = entity.Description;
                existingStatus.IsActive = entity.IsActive;
            }
            else
            {
                throw new ArgumentException("Статус бронирования не найден.");
            }
        }

        public Task UpdateAsync(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<BookingStatus>> FindAsync(Expression<Func<BookingStatus, bool>> predicate)
        {
            return Task.FromResult(bookingStatuses.AsQueryable().Where(predicate).ToList().AsEnumerable());
        }

        public IQueryable<BookingStatus> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return bookingStatuses.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public Task<BookingStatus> FirstOrDefaultAsync(Expression<Func<BookingStatus, bool>> predicate)
        {
            return Task.FromResult(bookingStatuses.AsQueryable().FirstOrDefault(predicate));
        }

        // Реализация метода Remove
        public void Remove(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bookingStatuses.Remove(entity);
        }

        public Task<bool> RemoveAsync(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Remove(entity);
            return Task.FromResult(true);
        }
    }
}