using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class DepartmentTestRepository : IRepository<Department>
    {
        private readonly List<Department> departments;

        public DepartmentTestRepository(List<Department> departments)
        {
            this.departments = departments ?? throw new ArgumentNullException(nameof(departments));
            SetupData();
        }

        private void SetupData()
        {
            if (!departments.Any())
            {
                departments.Add(new Department { DepartmentID = 1, DepartmentName = "Отдел 1" });
                departments.Add(new Department { DepartmentID = 2, DepartmentName = "Отдел 2" });
            }
        }

        public void Add(Department entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            departments.Add(entity);
        }

        public async Task AddAsync(Department entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            departments.Add(entity);
            await Task.CompletedTask; // Убираем Task.Run для простоты
        }

        public bool Delete(int id)
        {
            var department = Get(id);
            if (department == null) return false;
            return departments.Remove(department);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = Get(id);
            if (department == null) return false;
            departments.Remove(department);
            return await Task.FromResult(true); // Убираем Task.Run для простоты
        }

        public IQueryable<Department> Find(Expression<Func<Department, bool>> predicate) => departments.AsQueryable().Where(predicate);

        public Department Get(int id, params string[] includes) => departments.FirstOrDefault(d => d.DepartmentID == id);

        public async Task<Department> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes)); // Убираем Task.Run для простоты
        }

        public IQueryable<Department> GetAll() => departments.AsQueryable();

        public IQueryable<Department> GetAll(Expression<Func<Department, object>> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? departments.AsQueryable().OrderBy(orderBy) : departments.AsQueryable().OrderByDescending(orderBy);
        }

        public IQueryable<Department> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return departments.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public void Update(Department entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existingDepartment = Get(entity.DepartmentID);
            if (existingDepartment != null)
            {
                existingDepartment.DepartmentName = entity.DepartmentName;
            }
        }

        public async Task UpdateAsync(Department entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask; // Убираем Task.Run для простоты
        }

        public async Task<IEnumerable<Department>> FindAsync(Expression<Func<Department, bool>> predicate)
        {
            return await Task.FromResult(departments.AsQueryable().Where(predicate).ToList()); // Убираем Task.Run для простоты
        }
    }
}