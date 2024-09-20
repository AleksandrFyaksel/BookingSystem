using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem.Business.Managers
{
    public class ParkingSpaceManager : BaseManager
    {
        public ParkingSpaceManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<ParkingSpace> ParkingSpaces => parkingSpaceRepository.GetAll();

        public ParkingSpace GetParkingSpaceById(int id) => parkingSpaceRepository.Get(id);

        public ParkingSpace CreateParkingSpace(ParkingSpace parkingSpace)
        {
            parkingSpaceRepository.Add(parkingSpace);
            unitOfWork.SaveChanges();
            return parkingSpace;
        }

        public bool DeleteParkingSpace(int id)
        {
            var result = parkingSpaceRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateParkingSpace(ParkingSpace parkingSpace)
        {
            parkingSpaceRepository.Update(parkingSpace);
            unitOfWork.SaveChanges();
        }
    }
}