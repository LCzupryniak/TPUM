using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Data.Models
{
    internal class User : IUser
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }

        public User(int id, string name, string email, string address = "", string phoneNumber = "")
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
            IsActive = true;
        }

        public void UpdateContactInfo(string email, string address, string phoneNumber)
        {
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
