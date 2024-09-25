using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;

namespace BookingSystem.Business.Managers
{
    public class BaseManager
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IRepository<Booking> bookingRepository;
        protected readonly IRepository<Department> departmentRepository;
        protected readonly IRepository<Floor> floorRepository;
        protected readonly IRepository<Office> officeRepository;
        protected readonly IRepository<ParkingSpace> parkingSpaceRepository;
        protected readonly IRepository<Role> roleRepository;
        protected readonly IRepository<User> userRepository;
        protected readonly IRepository<UserPassword> userPasswordRepository;
        protected readonly IRepository<Workspace> workspaceRepository;
        protected readonly IRepository<BookingStatus> bookingStatusRepository;

        public BaseManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); // Проверка на null

            // Инициализация репозиториев
            bookingRepository = unitOfWork.BookingsRepository;
            departmentRepository = unitOfWork.DepartmentsRepository;
            floorRepository = unitOfWork.FloorsRepository;
            officeRepository = unitOfWork.OfficesRepository;
            parkingSpaceRepository = unitOfWork.ParkingSpacesRepository;
            roleRepository = unitOfWork.RolesRepository;
            userRepository = unitOfWork.UsersRepository;
            userPasswordRepository = unitOfWork.UserPasswordsRepository;
            workspaceRepository = unitOfWork.WorkspacesRepository;
            bookingStatusRepository = unitOfWork.BookingStatusesRepository;
        }
    }
}