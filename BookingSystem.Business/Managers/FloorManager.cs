using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem.Business.Managers
{
    public class FloorManager : BaseManager
    {
        public FloorManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<Floor> Floors => floorRepository.GetAll();

        public Floor GetFloorById(int id) => floorRepository.Get(id);

        public Floor CreateFloor(Floor floor)
        {
            floorRepository.Add(floor);
            unitOfWork.SaveChanges();
            return floor;
        }

        public bool DeleteFloor(int id)
        {
            var result = floorRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateFloor(Floor floor)
        {
            floorRepository.Update(floor);
            unitOfWork.SaveChanges();
        }
    }
}