using BookingSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace BookingSystem.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для единицы работы, обеспечивающий доступ к репозиториям.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Booking> BookingsRepository { get; }
        IRepository<Office> OfficesRepository { get; }
        IRepository<Floor> FloorsRepository { get; }
        IRepository<Workspace> WorkspacesRepository { get; }
        IRepository<ParkingSpace> ParkingSpacesRepository { get; }
        IRepository<User> UsersRepository { get; }
        IRepository<Department> DepartmentsRepository { get; }
        IRepository<Role> RolesRepository { get; }
        IRepository<UserPassword> UserPasswordsRepository { get; }
        IRepository<BookingStatus> BookingStatusesRepository { get; } // Исправлено на множественное число

        /// <summary>
        /// Сохраняет изменения в текущем контексте.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Асинхронно сохраняет изменения в текущем контексте.
        /// </summary>
        Task SaveChangesAsync();
    }
}