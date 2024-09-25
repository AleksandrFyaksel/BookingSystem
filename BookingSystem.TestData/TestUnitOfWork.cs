using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        }

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask; // Заглушка для асинхронного метода
        }

        public void Dispose()
        {
            // Освобождение ресурсов, если это необходимо
        }

        public bool HasChanges()
        {
            // Проверяем, есть ли изменения в тестовых репозиториях
            return bookings.Count > 0 || offices.Count > 0 || floors.Count > 0 ||
                   workspaces.Count > 0 || parkingSpaces.Count > 0 || users.Count > 0 ||
                   departments.Count > 0 || roles.Count > 0 || userPasswords.Count > 0 ||
                   bookingStatuses.Count > 0;
        }

        public void Rollback()
        {
            // Логика для отката изменений в тестовом окружении
            bookings.Clear();
            offices.Clear();
            floors.Clear();
            workspaces.Clear();
            parkingSpaces.Clear();
            users.Clear();
            departments.Clear();
            roles.Clear();
            userPasswords.Clear();
            bookingStatuses.Clear();
        }

        public IRepository<T> Set<T>() where T : class
        {
            // Возвращаем тестовый репозиторий для указанного типа
            if (typeof(T) == typeof(Booking))
                return (IRepository<T>)bookingsRepository;
            if (typeof(T) == typeof(Office))
                return (IRepository<T>)officesRepository;
            if (typeof(T) == typeof(Floor))
                return (IRepository<T>)floorsRepository;
            if (typeof(T) == typeof(Workspace))
                return (IRepository<T>)workspacesRepository;
            if (typeof(T) == typeof(ParkingSpace))
                return (IRepository<T>)parkingSpacesRepository;
            if (typeof(T) == typeof(User))
                return (IRepository<T>)usersRepository;
            if (typeof(T) == typeof(Department))
                return (IRepository<T>)departmentsRepository;
            if (typeof(T) == typeof(Role))
                return (IRepository<T>)rolesRepository;
            if (typeof(T) == typeof(UserPassword))
                return (IRepository<T>)userPasswordsRepository;
            if (typeof(T) == typeof(BookingStatus))
                return (IRepository<T>)bookingStatusesRepository;

            throw new NotImplementedException($"Репозиторий для типа {typeof(T).Name} не реализован.");
        }

        // Добавленный метод для асинхронного поиска
        public Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Set<T>().FirstOrDefaultAsync(predicate);
        }

        // Добавленные методы Remove и RemoveAsync
        public void Remove<T>(T entity) where T : class
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var repository = Set<T>();
            repository.Remove(entity);
        }

        public Task<bool> RemoveAsync<T>(T entity) where T : class
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var repository = Set<T>();
            repository.Remove(entity);
            return Task.FromResult(true);
        }
    }
}