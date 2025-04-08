using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace LogicTests
{
    internal class StubCustomer : ICustomer
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        public StubCustomer(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
