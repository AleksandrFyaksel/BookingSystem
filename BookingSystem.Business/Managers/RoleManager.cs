using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem.Business.Managers
{
    public class RoleManager : BaseManager
    {
        public RoleManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<Role> Roles => roleRepository.GetAll();

        public Role GetRoleById(int id) => roleRepository.Get(id);

        public Role CreateRole(Role role)
        {
            roleRepository.Add(role);
            unitOfWork.SaveChanges();
            return role;
        }

        public bool DeleteRole(int id)
        {
            var result = roleRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateRole(Role role)
        {
            roleRepository.Update(role);
            unitOfWork.SaveChanges();
        }
    }
}