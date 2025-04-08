using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace DataTests
{
    internal class DummyCustomer : ICustomer
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        public DummyCustomer(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
