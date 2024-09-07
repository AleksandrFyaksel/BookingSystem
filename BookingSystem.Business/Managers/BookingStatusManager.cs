using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Business.Managers
{
    public class BookingStatusManager
    {
        private readonly IRepository<BookingStatus> bookingStatusRepository;

        public BookingStatusManager(IUnitOfWork unitOfWork)
        {
            bookingStatusRepository = unitOfWork.BookingStatusesRepository; 
        }

        // Добавление нового статуса бронирования
        public async Task AddBookingStatusAsync(BookingStatus bookingStatus)
        {
            await bookingStatusRepository.AddAsync(bookingStatus);
        }

        // Удаление статуса бронирования по ID
        public async Task<bool> DeleteBookingStatusAsync(int id)
        {
            return await bookingStatusRepository.DeleteAsync(id);
        }

        // Получение статуса бронирования по ID
        public async Task<BookingStatus> GetBookingStatusByIdAsync(int id)
        {
            return await bookingStatusRepository.GetAsync(id);
        }

        // Получение всех статусов бронирования
        public async Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync()
        {
            return await bookingStatusRepository.GetAll().ToListAsync();
        }

        // Обновление статуса бронирования
        public async Task UpdateBookingStatusAsync(BookingStatus bookingStatus)
        {
            await bookingStatusRepository.UpdateAsync(bookingStatus);
        }

        // Синхронные методы для совместимости
        public void AddBookingStatus(BookingStatus bookingStatus) => AddBookingStatusAsync(bookingStatus).GetAwaiter().GetResult();
        public bool DeleteBookingStatus(int id) => DeleteBookingStatusAsync(id).GetAwaiter().GetResult();
        public BookingStatus GetBookingStatusById(int id) => GetBookingStatusByIdAsync(id).GetAwaiter().GetResult();

    }
}