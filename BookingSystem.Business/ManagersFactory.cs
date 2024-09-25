using BookingSystem.Domain.Interfaces; 
using BookingSystem.DAL.Data; 
using BookingSystem.Business.Managers; 
using BookingSystem.DAL.Repositories;

namespace BookingSystem.Business
{
    public class ManagersFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookingContext _context; // Поле для контекста

        public ManagersFactory(IUnitOfWork unitOfWork, BookingContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context; // Инициализация контекста
        }

        public BookingManager CreateBookingManager()
        {
            return new BookingManager(_unitOfWork, _context); // Передаем оба параметра
        }
    }
}