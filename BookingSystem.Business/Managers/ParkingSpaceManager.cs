using BookingSystem.DAL.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace BookingSystem.Business.Managers
{
    public class ParkingSpaceManager : BaseManager
    {
        public ParkingSpaceManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<ParkingSpace> ParkingSpaces => parkingSpaceRepository.GetAll();

        public ParkingSpace GetParkingSpaceById(int id) => parkingSpaceRepository.Get(id);

        public ParkingSpace CreateParkingSpace(ParkingSpace parkingSpace)
        {
            if (parkingSpace == null) throw new ArgumentNullException(nameof(parkingSpace)); // Проверка на null
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
            if (parkingSpace == null) throw new ArgumentNullException(nameof(parkingSpace)); // Проверка на null
            parkingSpaceRepository.Update(parkingSpace);
            unitOfWork.SaveChanges();
        }
    }
}