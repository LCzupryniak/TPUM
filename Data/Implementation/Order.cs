using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace Data.Implementation
{
    internal class Order : IOrder
    {
        public int Id { get; private set; }
        public ICustomer Customer { get; private set; }

        public Order(int id, ICustomer customer)
        {
            Id = id;
            Customer = customer;
        }
    }
}
