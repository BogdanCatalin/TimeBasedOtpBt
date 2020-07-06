using System.Collections.Generic;
using TimeBasedOTPBT.Persistence.Entities;

namespace TimeBasedOTPBT.BusinessLogic.Services.UserService
{
    public interface IUserService
    {
        object PreAuthenticate(string username, string password);
        bool Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
    }
}
