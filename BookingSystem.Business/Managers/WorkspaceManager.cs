using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem.Business.Managers
{
    public class WorkspaceManager : BaseManager
    {
        public WorkspaceManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<Workspace> Workspaces => workspaceRepository.GetAll();

        public Workspace GetWorkspaceById(int id) => workspaceRepository.Get(id);

        public Workspace CreateWorkspace(Workspace workspace)
        {
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
            workspaceRepository.Update(workspace);
            unitOfWork.SaveChanges();
        }
    }
}