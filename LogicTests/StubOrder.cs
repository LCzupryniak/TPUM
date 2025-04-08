using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace LogicTests
{
    internal class StubOrder : IOrder
    {
        public int Id { get; private set; }
        public ICustomer Customer { get; private set; }

        public StubOrder(int orderId, ICustomer customer)
        {
            Id = orderId;
            Customer = customer;
        }
    }
}
