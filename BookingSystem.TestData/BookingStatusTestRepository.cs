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

        public async Task AddAsync(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bookingStatuses.Add(entity);
            await Task.CompletedTask; 
        }

        public bool Delete(int id)
        {
            var bookingStatus = Get(id);
            if (bookingStatus == null) return false;
            return bookingStatuses.Remove(bookingStatus);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bookingStatus = Get(id);
            if (bookingStatus == null) return false;
            bookingStatuses.Remove(bookingStatus);
            return await Task.FromResult(true); 
        }

        public IQueryable<BookingStatus> Find(Expression<Func<BookingStatus, bool>> predicate) => bookingStatuses.AsQueryable().Where(predicate);

        public BookingStatus Get(int id, params string[] includes) => bookingStatuses.FirstOrDefault(bs => bs.BookingStatusID == id);

        public async Task<BookingStatus> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes)); 
        }

        public IQueryable<BookingStatus> GetAll() => bookingStatuses.AsQueryable();

        public IQueryable<BookingStatus> GetAll(Expression<Func<BookingStatus, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? bookingStatuses.AsQueryable().OrderBy(orderBy) : bookingStatuses.AsQueryable().OrderByDescending(orderBy);
        }

        public IQueryable<BookingStatus> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return bookingStatuses.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
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
        }

        public async Task UpdateAsync(BookingStatus entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask; 
        }

        public async Task<IEnumerable<BookingStatus>> FindAsync(Expression<Func<BookingStatus, bool>> predicate)
        {
            return await Task.FromResult(bookingStatuses.AsQueryable().Where(predicate).ToList()); // Убираем Task.Run для простоты
        }
    }
}