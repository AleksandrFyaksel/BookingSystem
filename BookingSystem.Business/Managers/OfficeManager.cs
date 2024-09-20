using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem.Business.Managers
{
    public class OfficeManager : BaseManager
    {
        public OfficeManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<Office> Offices => officeRepository.GetAll();

        public Office GetOfficeById(int id) => officeRepository.Get(id);

        public Office CreateOffice(Office office)
        {
            officeRepository.Add(office);
            unitOfWork.SaveChanges();
            return office;
        }

        public bool DeleteOffice(int id)
        {
            var result = officeRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateOffice(Office office)
        {
            officeRepository.Update(office);
            unitOfWork.SaveChanges();
        }
    }
}