using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class ParkingSpaceTestRepository : IRepository<ParkingSpace>
    {
        private readonly List<ParkingSpace> parkingSpaces;

        public ParkingSpaceTestRepository(List<ParkingSpace> parkingSpaces)
        {
            this.parkingSpaces = parkingSpaces ?? throw new ArgumentNullException(nameof(parkingSpaces));
            SetupData();
        }

        private void SetupData()
        {
            if (!parkingSpaces.Any())
            {
                parkingSpaces.Add(new ParkingSpace { ParkingSpaceID = 1, Position = "Парковка 1", IsAvailable = true, OfficeID = 1 });
                parkingSpaces.Add(new ParkingSpace { ParkingSpaceID = 2, Position = "Парковка 2", IsAvailable = false, OfficeID = 1 });
            }
        }

        public void Add(ParkingSpace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            parkingSpaces.Add(entity);
        }

        public async Task AddAsync(ParkingSpace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            parkingSpaces.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var parkingSpace = Get(id);
            return parkingSpace != null && parkingSpaces.Remove(parkingSpace);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parkingSpace = Get(id);
            if (parkingSpace == null) return false;
            parkingSpaces.Remove(parkingSpace);
            return await Task.FromResult(true);
        }

        public IQueryable<ParkingSpace> Find(Expression<Func<ParkingSpace, bool>> predicate) => parkingSpaces.AsQueryable().Where(predicate);

        public ParkingSpace Get(int id, params string[] includes) => parkingSpaces.FirstOrDefault(p => p.ParkingSpaceID == id);

        public async Task<ParkingSpace> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<ParkingSpace> GetAll() => parkingSpaces.AsQueryable();

        public IQueryable<ParkingSpace> GetAll(Func<ParkingSpace, object> orderBy, bool ascending = true)
        {
            return ascending ? parkingSpaces.OrderBy(orderBy).AsQueryable() : parkingSpaces.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<ParkingSpace> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return parkingSpaces.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<ParkingSpace> GetAll(Expression<Func<ParkingSpace, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return parkingSpaces.AsQueryable();
        }

        public void Update(ParkingSpace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existingParkingSpace = Get(entity.ParkingSpaceID);
            if (existingParkingSpace != null)
            {
                existingParkingSpace.Position = entity.Position;
                existingParkingSpace.IsAvailable = entity.IsAvailable;
            }
        }

        public async Task UpdateAsync(ParkingSpace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<ParkingSpace>> FindAsync(Expression<Func<ParkingSpace, bool>> predicate)
        {
            return await Task.FromResult(parkingSpaces.AsQueryable().Where(predicate).ToList());
        }
    }
}