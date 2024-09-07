using BookingSystem.Business.Managers;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Linq;

namespace BookingSystem.Business.Infrastructure
{
    public static class DbTestData
    {
        public static void SetupData(BookingManager bookingManager, UserManager userManager)
        {
            if (bookingManager == null) throw new ArgumentNullException(nameof(bookingManager));
            if (userManager == null) throw new ArgumentNullException(nameof(userManager));

            // Создание пользователей
            CreateUserIfNotExists(userManager, "admin", "admin@example.com", "hashedpassword", "1234567890");
            CreateUserIfNotExists(userManager, "user1", "user1@example.com", "hashedpassword", "0987654321");

            var users = userManager.Users.ToArray();

            // Создание бронирований
            CreateBookingIfNotExists(bookingManager, users.First(u => u.Email == "admin@example.com").UserID, 1, 1, new DateTime(2024, 10, 1), new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0), "Требуется проектор");
            CreateBookingIfNotExists(bookingManager, users.First(u => u.Email == "user1@example.com").UserID, 1, 1, new DateTime(2024, 10, 2), new TimeSpan(11, 0, 0), new TimeSpan(12, 0, 0), "Требуется доска");
        }

        private static void CreateUserIfNotExists(UserManager userManager, string name, string email, string passwordHash, string phoneNumber)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager));
            if (!userManager.Users.Any(u => u.Email == email)) // Проверка по email
            {
                userManager.CreateUser(new User
                {
                    Name = name, // Используем свойство Name
                    Email = email,
                    PasswordHash = passwordHash,
                    PhoneNumber = phoneNumber
                });
            }
        }

        private static void CreateBookingIfNotExists(BookingManager bookingManager, int userId, int workspaceId, int floorId, DateTime bookingDate, TimeSpan startTime, TimeSpan endTime, string additionalRequirements)
        {
            // Здесь можно добавить логику для проверки существования бронирования, если это необходимо
            bookingManager.CreateBooking(new Booking
            {
                UserID = userId,
                WorkspaceID = workspaceId, // Добавлено свойство WorkspaceID
                FloorID = floorId,
                BookingDate = bookingDate, // Замените Date на BookingDate
                StartDateTime = bookingDate.Date + startTime, // Устанавливаем время начала
                EndDateTime = bookingDate.Date + endTime, // Устанавливаем время окончания
                AdditionalRequirements = additionalRequirements
            });
        }
    }
}