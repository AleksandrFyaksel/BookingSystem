using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingSystem.DAL.Repositories;

namespace BookingSystem.Business.Managers
{
    public class BookingManager
    {
        private readonly BookingContext _context;
        private readonly IUnitOfWork unitOfWork;

        public BookingManager(IUnitOfWork unitOfWork, BookingContext context)
        {
            this.unitOfWork = unitOfWork;
            _context = context;
        }

        // Получение всех офисов
        public async Task<List<Office>> GetAllOfficesAsync()
        {
            return await _context.Offices.ToListAsync();
        }

        // Получение всех этажей с их офисами
        public async Task<List<Floor>> GetAllFloorsAsync()
        {
            return await _context.Floors.Include(f => f.Office).ToListAsync();
        }

        // Получение пути к изображению этажа по ID
        public async Task<string> GetFloorImagePathAsync(int floorId)
        {
            var floor = await _context.Floors.FindAsync(floorId);
            if (floor != null && !string.IsNullOrEmpty(floor.ImageMimeType))
            {
                // Формируем полный путь к изображению
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", floor.ImageMimeType);

                // Логирование пути для отладки
                Console.WriteLine($"Image path: {imagePath}");

                return imagePath;
            }
            return null; // Возвращаем null, если этаж не найден или имя файла пустое
        }

        // Получение названия файла изображения из базы данных
        public async Task<string> GetFloorImageFileNameAsync(int floorId)
        {
            var floor = await _context.Floors.FindAsync(floorId);
            return floor?.ImageMimeType; // Используем ImageMimeType как название файла
        }

        // Получение парковочных мест по ID этажа
        public async Task<List<ParkingSpace>> GetParkingSpacesByFloorIDAsync(int floorID)
        {
            return await _context.ParkingSpaces
                .Where(ps => ps.FloorID == floorID)
                .ToListAsync();
        }

        // Создание бронирования
        public async Task CreateBookingAsync(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }
            await unitOfWork.BookingsRepository.AddAsync(booking);
            await unitOfWork.SaveChangesAsync();
        }

        // Добавление рабочего места
        public async Task AddWorkspaceAsync(Workspace workspace)
        {
            await AddEntityAsync(workspace);
        }

        // Добавление парковочного места
        public async Task AddParkingSpaceAsync(ParkingSpace parkingSpace)
        {
            await AddEntityAsync(parkingSpace);
        }

        // Удаление рабочего места
        public async Task DeleteWorkspaceAsync(int workspaceId)
        {
            await DeleteEntityAsync<Workspace>(workspaceId);
        }

        // Удаление парковочного места
        public async Task DeleteParkingSpaceAsync(int parkingSpaceId)
        {
            await DeleteEntityAsync<ParkingSpace>(parkingSpaceId);
        }

        // Получение офиса по ID
        public async Task<Office> GetOfficeByIdAsync(int officeId)
        {
            return await _context.Offices.FindAsync(officeId);
        }

        // Получение этажа по ID
        public async Task<Floor> GetFloorByIdAsync(int floorId)
        {
            return await _context.Floors.FindAsync(floorId);
        }

        // Получение рабочего места по ID
        public async Task<Workspace> GetWorkspaceByIdAsync(int workspaceId)
        {
            return await _context.Workspaces.FindAsync(workspaceId);
        }

        // Получение парковочного места по ID
        public async Task<ParkingSpace> GetParkingSpaceByIdAsync(int parkingSpaceId)
        {
            return await _context.ParkingSpaces.FindAsync(parkingSpaceId);
        }

        // Получение этажей по ID офиса
        public async Task<List<Floor>> GetFloorsByOfficeIdAsync(int officeId)
        {
            return await _context.Floors
                .Where(f => f.OfficeID == officeId)
                .ToListAsync();
        }

        // Обобщенный метод для добавления сущностей
        private async Task AddEntityAsync<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                await unitOfWork.Set<T>().AddAsync(entity);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении сущности: {ex.Message}", ex);
            }
        }

        private async Task DeleteEntityAsync<T>(int id) where T : class
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID должен быть положительным числом.", nameof(id));
            }

            try
            {
                var dbSet = unitOfWork.Set<T>();
                if (dbSet == null)
                {
                    throw new InvalidOperationException("Не удалось получить доступ к набору данных.");
                }

                // Используем FirstOrDefaultAsync для поиска сущности по ID
                var entity = await dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    await unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Сущность с ID {id} не найдена.");
                }
            }
            catch (DbUpdateException dbEx)
            {
                // Логирование ошибки базы данных
                throw new Exception("Ошибка при обновлении базы данных.", dbEx);
            }
            catch (Exception ex)
            {
                // Логирование общей ошибки
                throw new Exception($"Ошибка при удалении сущности: {ex.Message}", ex);
            }
        }
    }
}