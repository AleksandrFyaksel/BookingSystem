using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace BookingSystem.Business.Managers
{
    public class WorkspaceManager : BaseManager
    {
        public WorkspaceManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<Workspace> Workspaces => workspaceRepository.GetAll();

        public Workspace GetWorkspaceById(int id) => workspaceRepository.Get(id);

        public Workspace CreateWorkspace(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace)); // Проверка на null
            workspaceRepository.Add(workspace);
            unitOfWork.SaveChanges();
            return workspace;
        }

        public bool DeleteWorkspace(int id)
        {
            var result = workspaceRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateWorkspace(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace)); // Проверка на null
            workspaceRepository.Update(workspace);
            unitOfWork.SaveChanges();
        }
    }
}