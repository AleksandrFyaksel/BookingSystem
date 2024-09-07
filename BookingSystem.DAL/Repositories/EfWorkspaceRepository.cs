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
    public class EfWorkspaceRepository : IRepository<Workspace>
    {
        private readonly BookingContext context;
        private readonly DbSet<Workspace> workspaces;

        public EfWorkspaceRepository(BookingContext context)
        {
            this.context = context;
            workspaces = context.Workspaces;
        }

        public async Task AddAsync(Workspace entity)
        {
            await workspaces.AddAsync(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Add(Workspace entity) => AddAsync(entity).GetAwaiter().GetResult();

        public async Task<bool> DeleteAsync(int id)
        {
            var workspace = await workspaces.FindAsync(id).ConfigureAwait(false);
            if (workspace == null) return false;
            workspaces.Remove(workspace);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public bool Delete(int id) => DeleteAsync(id).GetAwaiter().GetResult();

        public IQueryable<Workspace> Find(Expression<Func<Workspace, bool>> predicate)
        {
            return workspaces.Where(predicate);
        }

        public async Task<IEnumerable<Workspace>> FindAsync(Expression<Func<Workspace, bool>> predicate)
        {
            return await workspaces.Where(predicate).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Workspace> GetAsync(int id, params string[] includes)
        {
            IQueryable<Workspace> query = workspaces;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(ws => ws.WorkspaceID == id).ConfigureAwait(false);
        }

        public Workspace Get(int id, params string[] includes)
        {
            IQueryable<Workspace> query = workspaces;
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(ws => ws.WorkspaceID == id) ?? throw new KeyNotFoundException($"Workspace with ID {id} not found.");
        }

        public IQueryable<Workspace> GetAll()
        {
            return workspaces; // Возвращаем IQueryable напрямую
        }

        public IQueryable<Workspace> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return workspaces.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Workspace> GetAll(Expression<Func<Workspace, object>> orderBy, bool ascending = true)
        {
            return ascending ? workspaces.OrderBy(orderBy) : workspaces.OrderByDescending(orderBy);
        }

        public async Task UpdateAsync(Workspace entity)
        {
            workspaces.Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Update(Workspace entity) => UpdateAsync(entity).GetAwaiter().GetResult();
    }
}