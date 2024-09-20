using BookingSystem.Domain.Interfaces;
using BookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingSystem.DAL.Repositories;

namespace BookingSystem.TestData
{
    public class TestUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IRepository<Booking> bookingsRepository;
        private readonly IRepository<Office> officesRepository;
        private readonly IRepository<Floor> floorsRepository;
        private readonly IRepository<Workspace> workspacesRepository;
        private readonly IRepository<ParkingSpace> parkingSpacesRepository;
        private readonly IRepository<User> usersRepository;
        private readonly IRepository<Department> departmentsRepository;
        private readonly IRepository<Role> rolesRepository;
        private readonly IRepository<UserPassword> userPasswordsRepository;
        private readonly IRepository<BookingStatus> bookingStatusesRepository;

        private readonly List<Booking> bookings;
        private readonly List<Office> offices;
        private readonly List<Floor> floors;
        private readonly List<Workspace> workspaces;
        private readonly List<ParkingSpace> parkingSpaces;
        private readonly List<User> users;
        private readonly List<Department> departments;
        private readonly List<Role> roles;
        private readonly List<UserPassword> userPasswords;
        private readonly List<BookingStatus> bookingStatuses;

        public TestUnitOfWork()
        {
            bookings = new List<Booking>();
            offices = new List<Office>();
            floors = new List<Floor>();
            workspaces = new List<Workspace>();
            parkingSpaces = new List<ParkingSpace>();
            users = new List<User>();
            departments = new List<Department>();
            roles = new List<Role>();
            userPasswords = new List<UserPassword>();
            bookingStatuses = new List<BookingStatus>();

            bookingsRepository = new BookingTestRepository(bookings);
            officesRepository = new OfficeTestRepository(offices);
            floorsRepository = new FloorTestRepository(floors);
            workspacesRepository = new WorkspaceTestRepository(workspaces);
            parkingSpacesRepository = new ParkingSpaceTestRepository(parkingSpaces);
            usersRepository = new UserTestRepository(users);
            departmentsRepository = new DepartmentTestRepository(departments);
            rolesRepository = new RoleTestRepository(roles);
            userPasswordsRepository = new UserPasswordTestRepository(userPasswords);
            bookingStatusesRepository = new BookingStatusTestRepository(bookingStatuses);
        }

        public IRepository<Booking> BookingsRepository => bookingsRepository;
        public IRepository<Office> OfficesRepository => officesRepository;
        public IRepository<Floor> FloorsRepository => floorsRepository;
        public IRepository<Workspace> WorkspacesRepository => workspacesRepository;
        public IRepository<ParkingSpace> ParkingSpacesRepository => parkingSpacesRepository;
        public IRepository<User> UsersRepository => usersRepository;
        public IRepository<Department> DepartmentsRepository => departmentsRepository;
        public IRepository<Role> RolesRepository => rolesRepository;
        public IRepository<UserPassword> UserPasswordsRepository => userPasswordsRepository;
        public IRepository<BookingStatus> BookingStatusesRepository => bookingStatusesRepository;

        public void SaveChanges()
        {
            // Логика для сохранения изменений в тестовом окружении
            // В данном случае, так как это тестовый репозиторий, можно оставить пустым
        }

        public async Task SaveChangesAsync()
        {
            // Логика для асинхронного сохранения изменений в тестовом окружении
            await Task.CompletedTask; // Заглушка для асинхронного метода
        }

        public void Dispose()
        {
            // Освобождение ресурсов, если это необходимо
            var repositories = new IDisposable[]
            {
        (IDisposable)bookingsRepository,
        (IDisposable)officesRepository,
        (IDisposable)floorsRepository,
        (IDisposable)workspacesRepository,
        (IDisposable)parkingSpacesRepository,
        (IDisposable)usersRepository,
        (IDisposable)departmentsRepository,
        (IDisposable)rolesRepository,
        (IDisposable)userPasswordsRepository,
        (IDisposable)bookingStatusesRepository
            };

            foreach (var repo in repositories)
            {
                repo.Dispose(); // Освобождение ресурсов
            }

            // Вызываем метод Dispose для освобождения управляемых ресурсов
            GC.SuppressFinalize(this);
        }

        public bool HasChanges()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}