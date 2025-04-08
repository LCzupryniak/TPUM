using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace Data.Implementation
{
    internal class Customer : ICustomer
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
