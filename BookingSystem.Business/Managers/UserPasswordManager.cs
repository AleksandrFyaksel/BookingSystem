using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem.Business.Managers
{
    public class UserPasswordManager : BaseManager
    {
        public UserPasswordManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<UserPassword> UserPasswords => userPasswordRepository.GetAll();

        public UserPassword GetUserPasswordById(int id) => userPasswordRepository.Get(id);

        public UserPassword CreateUserPassword(UserPassword userPassword)
        {
            userPasswordRepository.Add(userPassword);
            unitOfWork.SaveChanges();
            return userPassword;
        }

        public bool DeleteUserPassword(int id)
        {
            var result = userPasswordRepository.Delete(id);
            if (!result) return false;
            unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateUserPassword(UserPassword userPassword)
        {
            userPasswordRepository.Update(userPassword);
            unitOfWork.SaveChanges();
        }
    }
}