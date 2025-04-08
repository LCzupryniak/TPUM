using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace LogicTests
{
    internal class StubDataRepository : IDataRepository
    {
        private List<IProduct> _products;
        private List<ICustomer> _customers;
        private List<IOrder> _orders;

        public StubDataRepository()
        {
            _products = new List<IProduct>
            {
                new StubProduct("Produkt testowy 1", 100.0),
                new StubProduct("Produkt testowy 2", 200.0),
                new StubProduct("Produkt testowy 3", 300.0)
            };

            _customers = new List<ICustomer>
            {
                new StubCustomer("Testowy Klient", "test@example.com")
            };

            _orders = new List<IOrder>
            {
                new StubOrder(123, _customers[0])
            };
        }

        public List<IProduct> GetAllProducts()
        {
            return _products;
        }

        public List<ICustomer> GetAllCustomers()
        {
            return _customers;
        }

        public List<IOrder> GetAllOrders()
        {
            return _orders;
        }
    }
}
