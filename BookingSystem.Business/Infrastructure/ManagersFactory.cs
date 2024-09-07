using BookingSystem.Business.Managers;
using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Interfaces;
using BookingSystem.TestData;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BookingSystem.Business.Infrastructure
{
    public class ManagersFactory : IDisposable
    {
        public readonly IUnitOfWork unitOfWork;
        private BookingManager bookingManager; // Это поле не инициализируется сразу
        private UserManager userManager;

        // Конструктор для тестового UnitOfWork
        public ManagersFactory()
        {
            unitOfWork = new TestUnitOfWork(); // Используется тестовый UnitOfWork
        }

        // Конструктор для реального подключения к базе данных
        public ManagersFactory(string connStringName)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connString = configuration.GetConnectionString(connStringName);
                unitOfWork = new EFUnitOfWork(connString);
            }
            catch (Exception ex)
            {
                // Обработка исключений при загрузке конфигурации или подключении к базе данных
                throw new InvalidOperationException("Не удалось инициализировать ManagersFactory", ex);
            }
        }

        // Получение экземпляра BookingManager
        public BookingManager GetBookingManager()
        {
            return bookingManager ??= new BookingManager(unitOfWork);
        }

        // Получение экземпляра UserManager
        public UserManager GetUserManager()
        {
            return userManager ??= new UserManager(unitOfWork);
        }

        // Освобождение ресурсов
        public void Dispose()
        {
            unitOfWork?.Dispose(); // Освобождаем ресурсы UnitOfWork
        }
    }
}