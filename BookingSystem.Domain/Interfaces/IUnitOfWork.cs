using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;

namespace BookingSystem.DAL.Repositories
{
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

        bool HasChanges();
        void Rollback();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}