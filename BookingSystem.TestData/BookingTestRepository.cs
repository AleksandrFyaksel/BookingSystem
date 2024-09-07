using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class BookingTestRepository : IRepository<Booking>
    {
        private readonly List<Booking> bookings;

        public BookingTestRepository(List<Booking> bookings)
        {
            this.bookings = bookings ?? throw new ArgumentNullException(nameof(bookings));
            SetupData();
        }

        private void SetupData()
        {
            var rnd = new Random();
            for (var i = 1; i <= 5; i++)
            {
                bookings.Add(new Booking
                {
                    BookingID = i,
                    BookingDate = DateTime.Now.AddDays(rnd.Next(1, 10)),
                    StartDateTime = DateTime.Now.AddHours(rnd.Next(8, 17)),
                    EndDateTime = DateTime.Now.AddHours(rnd.Next(17, 20)),
                    UserID = rnd.Next(1, 3),
                    WorkspaceID = rnd.Next(1, 3),
                    ParkingSpaceID = rnd.Next(1, 3),
                    FloorID = rnd.Next(1, 3),
                    AdditionalRequirements = $"Дополнительные требования {i}"
                });
            }
        }

        public void Add(Booking entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bookings.Add(entity);
        }

        public async Task AddAsync(Booking entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bookings.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var booking = Get(id);
            if (booking == null) return false;
            return bookings.Remove(booking);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = Get(id);
            if (booking == null) return false;
            bookings.Remove(booking);
            return await Task.FromResult(true);
        }

        public IQueryable<Booking> Find(Expression<Func<Booking, bool>> predicate) => bookings.AsQueryable().Where(predicate);

        public Booking Get(int id, params string[] includes) => bookings.FirstOrDefault(b => b.BookingID == id);

        public async Task<Booking> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<Booking> GetAll() => bookings.AsQueryable();

        public IQueryable<Booking> GetAll(Func<Booking, object> orderBy, bool ascending = true)
        {
            return ascending ? bookings.OrderBy(orderBy).AsQueryable() : bookings.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Booking> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return bookings.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<Booking> GetAll(Expression<Func<Booking, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return bookings.AsQueryable();
        }

        public void Update(Booking entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existingBooking = Get(entity.BookingID);
            if (existingBooking != null)
            {
                existingBooking.BookingDate = entity.BookingDate;
                existingBooking.StartDateTime = entity.StartDateTime;
                existingBooking.EndDateTime = entity.EndDateTime;
                existingBooking.WorkspaceID = entity.WorkspaceID;
                existingBooking.FloorID = entity.FloorID;
                existingBooking.UserID = entity.UserID;
                existingBooking.AdditionalRequirements = entity.AdditionalRequirements;
                existingBooking.BookingStatusID = entity.BookingStatusID;
                existingBooking.ParkingSpaceID = entity.ParkingSpaceID;
            }
        }

        public async Task UpdateAsync(Booking entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Booking>> FindAsync(Expression<Func<Booking, bool>> predicate)
        {
            return await Task.FromResult(bookings.AsQueryable().Where(predicate).ToList());
        }
    }
}