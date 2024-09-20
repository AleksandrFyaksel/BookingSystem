using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem.Business.Managers
{
    public class DepartmentManager : BaseManager
    {
        public DepartmentManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<Department> Departments => departmentRepository.GetAll();

        public Department GetDepartmentById(int id) => departmentRepository.Get(id);

        public Department CreateDepartment(Department department)
        {
            departmentRepository.Add(department);
            unitOfWork.SaveChanges();
            return department;
        }

        public bool DeleteDepartment(int id)
        {
            var result = departmentRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateDepartment(Department department)
        {
            departmentRepository.Update(department);
            unitOfWork.SaveChanges();
        }
    }
}