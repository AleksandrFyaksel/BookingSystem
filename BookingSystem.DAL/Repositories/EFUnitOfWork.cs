using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BookingContext _context;

        // Репозитории
        private IRepository<Booking> _bookingsRepository;
        private IRepository<Department> _departmentsRepository;
        private IRepository<Floor> _floorsRepository;
        private IRepository<Office> _officesRepository;
        private IRepository<ParkingSpace> _parkingSpacesRepository;
        private IRepository<Role> _rolesRepository;
        private IRepository<User> _usersRepository;
        private IRepository<UserPassword> _userPasswordsRepository;
        private IRepository<Workspace> _workspacesRepository;
        private IRepository<BookingStatus> _bookingStatusesRepository;

        public EFUnitOfWork(BookingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<Booking> BookingsRepository =>
            _bookingsRepository ??= new EfBookingRepository(_context);

        public IRepository<Department> DepartmentsRepository =>
            _departmentsRepository ??= new EfDepartmentRepository(_context);

        public IRepository<Floor> FloorsRepository =>
            _floorsRepository ??= new EfFloorRepository(_context);

        public IRepository<Office> OfficesRepository =>
            _officesRepository ??= new EfOfficeRepository(_context);

        public IRepository<ParkingSpace> ParkingSpacesRepository =>
            _parkingSpacesRepository ??= new EfParkingSpaceRepository(_context);

        public IRepository<Role> RolesRepository =>
            _rolesRepository ??= new EfRoleRepository(_context);

        public IRepository<User> UsersRepository =>
            _usersRepository ??= new EfUserRepository(_context);

        public IRepository<UserPassword> UserPasswordsRepository =>
            _userPasswordsRepository ??= new EfUserPasswordRepository(_context);

        public IRepository<Workspace> WorkspacesRepository =>
            _workspacesRepository ??= new EfWorkspaceRepository(_context);

        public IRepository<BookingStatus> BookingStatusesRepository =>
            _bookingStatusesRepository ??= new EfBookingStatusRepository(_context);

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при сохранении изменений в базе данных.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при асинхронном сохранении изменений в базе данных.", ex);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Rollback()
        {
            
            throw new NotImplementedException("Rollback не реализован.");
        }

        public IRepository<T> Set<T>() where T : class
        {
           
            return new GenericRepository<T>(_context);
        }

        public async Task<Booking> GetBookingByConditionAsync(Expression<Func<Booking, bool>> predicate)
        {
            return await BookingsRepository.FirstOrDefaultAsync(predicate);
        }
    }
}