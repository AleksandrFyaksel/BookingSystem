using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookingSystem.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BookingContext context;

        // Репозитории
        private IRepository<Booking> bookingsRepository;
        private IRepository<Department> departmentsRepository;
        private IRepository<Floor> floorsRepository;
        private IRepository<Office> officesRepository;
        private IRepository<ParkingSpace> parkingSpacesRepository;
        private IRepository<Role> rolesRepository;
        private IRepository<User> usersRepository;
        private IRepository<UserPassword> userPasswordsRepository;
        private IRepository<Workspace> workspacesRepository;
        private IRepository<BookingStatus> bookingStatusesRepository;

        public EFUnitOfWork(string connectionString)
        {
            var options = new DbContextOptionsBuilder<BookingContext>()
                .UseSqlServer(connectionString)
                .Options;
            context = new BookingContext(options);
            context.Database.EnsureCreated();
        }

        public IRepository<Booking> BookingsRepository =>
            bookingsRepository ??= new EfBookingRepository(context);

        public IRepository<Department> DepartmentsRepository =>
            departmentsRepository ??= new EfDepartmentRepository(context);

        public IRepository<Floor> FloorsRepository =>
            floorsRepository ??= new EfFloorRepository(context);

        public IRepository<Office> OfficesRepository =>
            officesRepository ??= new EfOfficeRepository(context);

        public IRepository<ParkingSpace> ParkingSpacesRepository =>
            parkingSpacesRepository ??= new EfParkingSpaceRepository(context);

        public IRepository<Role> RolesRepository =>
            rolesRepository ??= new EfRoleRepository(context);

        public IRepository<User> UsersRepository =>
            usersRepository ??= new EfUserRepository(context);

        public IRepository<UserPassword> UserPasswordsRepository =>
            userPasswordsRepository ??= new EfUserPasswordRepository(context);

        public IRepository<Workspace> WorkspacesRepository =>
            workspacesRepository ??= new EfWorkspaceRepository(context);

        public IRepository<BookingStatus> BookingStatusesRepository =>
            bookingStatusesRepository ??= new EfBookingStatusRepository(context);

        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Обработка исключения, например, логирование
                throw new Exception("Ошибка при сохранении изменений в базе данных.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Обработка исключения, например, логирование
                throw new Exception("Ошибка при асинхронном сохранении изменений в базе данных.", ex);
            }
        }

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}