using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace BookingSystem.Business.Managers
{
    public class UserManager : BaseManager
    {
        private readonly IRepository<User> userRepository;

        public UserManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            userRepository = unitOfWork.UsersRepository; 
        }

        public IEnumerable<User> Users => userRepository.GetAll();

        public User GetUserById(int id) => userRepository.Get(id);

        public User CreateUser(User user) 
        {
            userRepository.Add(user);
            unitOfWork.SaveChanges();
            return user;
        }

        public bool DeleteUser(int id)
        {
            var result = userRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateUser(User user)
        {
            userRepository.Update(user);
            unitOfWork.SaveChanges();
        }
    }
}