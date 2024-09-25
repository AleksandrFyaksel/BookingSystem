using BookingSystem.Business.Managers;
using BookingSystem.DAL.Data;
using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BookingSystem.Business.Infrastructure
{
    public class ManagersFactory : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookingContext _context; // Добавлено поле для контекста
        private BookingManager _bookingManager;
        private UserManager _userManager;

        // Конструктор для реального подключения к базе данных
        public ManagersFactory(IUnitOfWork unitOfWork, BookingContext context)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Получение экземпляра BookingManager
        public BookingManager GetBookingManager()
        {
            return _bookingManager ??= new BookingManager(_unitOfWork, _context); // Передаем IUnitOfWork и BookingContext
        }

        // Получение экземпляра UserManager
        public UserManager GetUserManager()
        {
            return _userManager ??= new UserManager(_unitOfWork); // Используем IUnitOfWork
        }

        // Освобождение ресурсов
        public void Dispose()
        {
            _unitOfWork?.Dispose(); // Освобождаем ресурсы UnitOfWork
            _context?.Dispose(); // Освобождаем ресурсы контекста
        }
    }
}