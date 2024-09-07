using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingSystem.Business.Managers
{
    public class BookingManager : BaseManager
    {
        private readonly IRepository<Booking> bookingRepository;
        private readonly IRepository<ParkingSpace> parkingRepository; // Репозиторий для парковочных мест
        private readonly IRepository<Workspace> workspaceRepository; // Репозиторий для рабочих мест

        public BookingManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            bookingRepository = unitOfWork.BookingsRepository;
            parkingRepository = unitOfWork.ParkingSpacesRepository; // Инициализация
            workspaceRepository = unitOfWork.WorkspacesRepository; // Инициализация
        }

        public IEnumerable<Booking> Bookings => bookingRepository.GetAll();

        public Booking GetBookingById(int id) => bookingRepository.Get(id);

        public Booking CreateBooking(Booking booking)
        {
            bookingRepository.Add(booking);
            unitOfWork.SaveChanges();
            return booking;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            await bookingRepository.AddAsync(booking); // Асинхронное добавление
            await unitOfWork.SaveChangesAsync(); // Асинхронное сохранение изменений
            return booking;
        }

        public bool DeleteBooking(int id)
        {
            var result = bookingRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateBooking(Booking booking)
        {
            bookingRepository.Update(booking);
            unitOfWork.SaveChanges();
        }

        public async Task<ParkingSpace> AddParkingSpaceAsync(ParkingSpace parkingSpace)
        {
            await parkingRepository.AddAsync(parkingSpace);
            await unitOfWork.SaveChangesAsync();
            return parkingSpace;
        }

        public async Task<Workspace> AddWorkspaceAsync(Workspace workspace)
        {
            await workspaceRepository.AddAsync(workspace);
            await unitOfWork.SaveChangesAsync();
            return workspace;
        }

        // Метод для удаления парковочного места
        public async Task<bool> DeleteParkingSpaceAsync(int id)
        {
            var result = await parkingRepository.DeleteAsync(id);
            if (!result) return false;
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        // Метод для удаления рабочего места
        public async Task<bool> DeleteWorkspaceAsync(int id)
        {
            var result = await workspaceRepository.DeleteAsync(id);
            if (!result) return false;
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}