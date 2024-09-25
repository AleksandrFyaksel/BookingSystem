﻿using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.TestData
{
    public class WorkspaceTestRepository : IRepository<Workspace>
    {
        private readonly List<Workspace> workspaces;

        public WorkspaceTestRepository(List<Workspace> workspaces)
        {
            this.workspaces = workspaces ?? throw new ArgumentNullException(nameof(workspaces));
            SetupData();
        }

        private void SetupData()
        {
            if (!workspaces.Any())
            {
                // Пример добавления рабочих мест
                workspaces.Add(new Workspace { WorkspaceID = 1, PositionX = 0, PositionY = 0, IsAvailable = true, FloorID = 1 });
                workspaces.Add(new Workspace { WorkspaceID = 2, PositionX = 1, PositionY = 1, IsAvailable = false, FloorID = 1 });
            }
        }

        public void Add(Workspace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            workspaces.Add(entity);
        }

        public async Task AddAsync(Workspace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            workspaces.Add(entity);
            await Task.CompletedTask;
        }

        public bool Delete(int id)
        {
            var workspace = Get(id);
            return workspace != null && workspaces.Remove(workspace);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workspace = Get(id);
            if (workspace == null) return false;
            workspaces.Remove(workspace);
            return await Task.FromResult(true);
        }

        public IQueryable<Workspace> Find(Expression<Func<Workspace, bool>> predicate) => workspaces.AsQueryable().Where(predicate);

        public Workspace Get(int id, params string[] includes) => workspaces.FirstOrDefault(w => w.WorkspaceID == id);

        public async Task<Workspace> GetAsync(int id, params string[] includes)
        {
            return await Task.FromResult(Get(id, includes));
        }

        public IQueryable<Workspace> GetAll() => workspaces.AsQueryable();

        public IQueryable<Workspace> GetAll(Func<Workspace, object> orderBy, bool ascending = true)
        {
            return ascending ? workspaces.OrderBy(orderBy).AsQueryable() : workspaces.OrderByDescending(orderBy).AsQueryable();
        }

        public IQueryable<Workspace> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше нуля.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше нуля.");

            return workspaces.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IQueryable<Workspace> GetAll(Expression<Func<Workspace, object>> include, bool asNoTracking)
        {
            // Логика для обработки include и asNoTracking
            return workspaces.AsQueryable();
        }

        public void Update(Workspace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existingWorkspace = Get(entity.WorkspaceID);
            if (existingWorkspace != null)
            {
                existingWorkspace.PositionX = entity.PositionX;
                existingWorkspace.PositionY = entity.PositionY;
                existingWorkspace.IsAvailable = entity.IsAvailable;
                existingWorkspace.FloorID = entity.FloorID;
            }
        }

        public async Task UpdateAsync(Workspace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Update(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Workspace>> FindAsync(Expression<Func<Workspace, bool>> predicate)
        {
            return await Task.FromResult(workspaces.AsQueryable().Where(predicate).ToList());
        }

        public IQueryable<Workspace> GetAll(Expression<Func<Workspace, bool>> filter, Expression<Func<Workspace, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<Workspace> FirstOrDefaultAsync(Expression<Func<Workspace, bool>> predicate)
        {
            return Task.FromResult(workspaces.AsQueryable().FirstOrDefault(predicate));
        }

        // Реализация метода Remove
        public void Remove(Workspace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            workspaces.Remove(entity);
        }

        public Task<bool> RemoveAsync(Workspace entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Remove(entity);
            return Task.FromResult(true);
        }
    }
}