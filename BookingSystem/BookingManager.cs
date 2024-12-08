using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Business.Managers
{
    public class BookingManager
    {
        private readonly BookingContext _context;

        public BookingManager(BookingContext context)
        {
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
            return await _context.Floors.Include(f => f.Office).ToListAsync(); // Загружаем этажи с офисами
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
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        // Добавление рабочего места
        public async Task AddWorkspaceAsync(Workspace workspace)
        {
            await _context.Workspaces.AddAsync(workspace);
            await _context.SaveChangesAsync();
        }

        // Добавление парковочного места
        public async Task AddParkingSpaceAsync(ParkingSpace parkingSpace)
        {
            await _context.ParkingSpaces.AddAsync(parkingSpace);
            await _context.SaveChangesAsync();
        }

        // Удаление рабочего места
        public async Task DeleteWorkspaceAsync(int workspaceId)
        {
            var workspace = await _context.Workspaces.FindAsync(workspaceId);
            if (workspace != null)
            {
                _context.Workspaces.Remove(workspace);
                await _context.SaveChangesAsync();
            }
        }

        // Удаление парковочного места
        public async Task DeleteParkingSpaceAsync(int parkingSpaceId)
        {
            var parkingSpace = await _context.ParkingSpaces.FindAsync(parkingSpaceId);
            if (parkingSpace != null)
            {
                _context.ParkingSpaces.Remove(parkingSpace);
                await _context.SaveChangesAsync();
            }
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

        
    }
}