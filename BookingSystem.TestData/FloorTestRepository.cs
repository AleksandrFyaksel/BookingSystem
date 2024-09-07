using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class FloorTestRepository : IRepository<Floor>
    {
        private readonly List<Floor> floors;

        public FloorTestRepository(List<Floor> floors)
        {
            this.floors = floors ?? throw new ArgumentNullException(nameof(floors));
            SetupData();
        }

        private void SetupData()
        {
            if (!floors.Any())
            {
                floors.Add(new Floor
                {
                    FloorID = 1,
                    FloorName = "Этаж 1",
                    OfficeID = 1,
                    ImageData = null,
                    MimeType = null
                });
                floors.Add(new Floor
                {
                    FloorID = 2,
                    FloorName = "Этаж 2",
                    OfficeID = 1,
                    ImageData = null,
                    MimeType = null
                });
                floors.Add(new Floor
                {
                    FloorID = 3,
                    FloorName = "Этаж 3",
                    OfficeID = 2,
                    ImageData = null,
                    MimeType = null
                });
            }
        }

        public void Add(Floor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            floors.Add(entity);
        }

        public async Task AddAsync(Floor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            floors.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var floor = Get(id);
            if (floor == null) return false;
            return floors.Remove(floor);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var floor = Get(id);
            if (floor == null) return false;
            floors.Remove(floor);
            return await Task.FromResult(true);
        }

        public IQueryable<Floor> Find(Expression<Func<Floor, bool>> predicate) => floors.AsQueryable().Where(predicate);

        public Floor Get(int id, params string[] includes) => floors.FirstOrDefault(f => f.FloorID == id);

        public async Task<Floor> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<Floor> GetAll() => floors.AsQueryable();

        public IQueryable<Floor> GetAll(Func<Floor, object> orderBy, bool ascending = true)
        {
            return ascending ? floors.OrderBy(orderBy).AsQueryable() : floors.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Floor> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return floors.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<Floor> GetAll(Expression<Func<Floor, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return floors.AsQueryable();
        }

        public void Update(Floor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existingFloor = Get(entity.FloorID);
            if (existingFloor != null)
            {
                existingFloor.FloorName = entity.FloorName;
                existingFloor.ImageData = entity.ImageData;
                existingFloor.MimeType = entity.MimeType;
            }
        }

        public async Task UpdateAsync(Floor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Floor>> FindAsync(Expression<Func<Floor, bool>> predicate)
        {
            return await Task.FromResult(floors.AsQueryable().Where(predicate).ToList());
        }
    }
}