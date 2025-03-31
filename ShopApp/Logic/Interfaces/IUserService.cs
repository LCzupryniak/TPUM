using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Logic.Interfaces
{
    public interface IUserService
    {
        IUser GetUserById(int id);
        IUser GetUserByEmail(string email);
        void RegisterUser(string name, string email, string address, string phoneNumber);
        void UpdateUserStatus(int userId, bool isActive);
    }
}
