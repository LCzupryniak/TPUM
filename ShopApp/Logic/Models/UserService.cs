using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Logic.Interfaces;

namespace Logic.Models
{
    internal class UserService : IUserService
    {
        private Dictionary<int, IUser> _users = new();
        private Dictionary<string, int> _emailIndex = new();
        private int _nextUserId = 1;

        public IUser GetUserById(int id)
        {
            return _users.TryGetValue(id, out var user) ? user : null;
        }

        public IUser GetUserByEmail(string email)
        {
            return _emailIndex.TryGetValue(email, out var userId) && _users.TryGetValue(userId, out var user) ? user : null;
        }

        public void RegisterUser(string name, string email, string address, string phoneNumber)
        {
            if (_emailIndex.ContainsKey(email))
            {
                throw new InvalidOperationException("Użytkownik o podanym adresie email już istnieje.");
            }

            var newUser = new IUser(_nextUserId, name, email, address, phoneNumber);
            _users.Add(_nextUserId, newUser);
            _emailIndex.Add(email, _nextUserId);
            _nextUserId++;
        }

        public void UpdateUserStatus(int userId, bool isActive)
        {
            if (_users.TryGetValue(userId, out var user))
            {
                user.SetActiveStatus(isActive);
            }
        }
    }
}
