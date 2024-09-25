using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace BookingSystem.DAL.Repositories
{
    /// <summary>
    /// Интерфейс для работы с единицей работы.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Booking> BookingsRepository { get; }
        IRepository<BookingStatus> BookingStatusesRepository { get; }
        IRepository<Department> DepartmentsRepository { get; }
        IRepository<Floor> FloorsRepository { get; }
        IRepository<Office> OfficesRepository { get; }
        IRepository<ParkingSpace> ParkingSpacesRepository { get; }
        IRepository<Role> RolesRepository { get; }
        IRepository<UserPassword> UserPasswordsRepository { get; }
        IRepository<User> UsersRepository { get; }
        IRepository<Workspace> WorkspacesRepository { get; }

        /// <summary>
        /// Проверяет, есть ли изменения в контексте.
        /// </summary>
        /// <returns>True, если есть изменения; иначе - false.</returns>
        bool HasChanges();

        /// <summary>
        /// Откат изменений в контексте.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Сохраняет изменения в базе данных.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Асинхронно сохраняет изменения в базе данных.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Получает репозиторий для указанного типа сущности.
        /// </summary>
        IRepository<T> Set<T>() where T : class;
    }
}