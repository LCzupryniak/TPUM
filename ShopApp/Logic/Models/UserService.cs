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
        private Dictionary<int, User> _users;
        private Dictionary<string, int> _emailIndex;
        private int _nextUserId;

        public UserService()
        {
            _users = new Dictionary<int, User>();
            _emailIndex = new Dictionary<string, int>();
            _nextUserId = 1;
        }

        public IUser GetUserById(int id)
        {
            if (_users.TryGetValue(id, out var user))
            {
                return new ConcreteUser(user.Id, user.Name, user.Email, user.Address, user.PhoneNumber);
            }
            return null;
        }

        public IUser GetUserByEmail(string email)
        {
            if (_emailIndex.TryGetValue(email, out var userId) && _users.TryGetValue(userId, out var user))
            {
                return new ConcreteUser(user.Id, user.Name, user.Email, user.Address, user.PhoneNumber);
            }
            return null;
        }

        public void RegisterUser(string name, string email, string address, string phoneNumber)
        {
            // Sprawdzenie, czy użytkownik o podanym adresie email już istnieje
            if (_emailIndex.ContainsKey(email))
            {
                throw new InvalidOperationException("Użytkownik o podanym adresie email już istnieje.");
            }

            var newUser = new User(_nextUserId, name, email, address, phoneNumber);
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
