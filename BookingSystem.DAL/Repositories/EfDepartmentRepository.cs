using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.DAL.Repositories
{
    public class EfDepartmentRepository : IRepository<Department>
    {
        private readonly BookingContext context;
        private readonly DbSet<Department> departments;

        public EfDepartmentRepository(BookingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            departments = context.Departments;
        }

        public async Task AddAsync(Department entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await departments.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await departments.FindAsync(id);
            if (department == null) return false;
            departments.Remove(department);
            await context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Department> Find(Expression<Func<Department, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return departments.Where(predicate);
        }

        public async Task<IEnumerable<Department>> FindAsync(Expression<Func<Department, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await departments.Where(predicate).ToListAsync();
        }

        public async Task<Department> GetAsync(int id, params string[] includes)
        {
            IQueryable<Department> query = departments;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(d => d.DepartmentID == id);
        }

        public Department Get(int id, params string[] includes)
        {
            return GetAsync(id, includes).GetAwaiter().GetResult();
        }

        public IQueryable<Department> GetAll()
        {
            return departments.AsQueryable();
        }

        public IQueryable<Department> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return departments.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Department> GetAll(Func<Department, object> orderBy, bool ascending = true)
        {
            if (orderBy == null) throw new ArgumentNullException(nameof(orderBy));
            return ascending ? departments.OrderBy(orderBy).AsQueryable() : departments.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Department> GetAll(Expression<Func<Department, object>> include, bool asNoTracking)
        {
            IQueryable<Department> query = departments;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query.Include(include);
        }

        public async Task UpdateAsync(Department entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            departments.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Add(Department entity) => AddAsync(entity).GetAwaiter().GetResult();
        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();
        public void Update(Department entity) => UpdateAsync(entity).GetAwaiter().GetResult();

        public IQueryable<Department> GetAll(Expression<Func<Department, bool>> filter, Expression<Func<Department, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}