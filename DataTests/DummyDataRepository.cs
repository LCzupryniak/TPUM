using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace DataTests
{
    internal class DummyDataRepository : IDataRepository
    {
        public List<IProduct> GetAllProducts()
        {
            return new List<IProduct>
            {
                new DummyProduct("Telewizor SAMSUNG QLED 4K ", 2999.99),
                new DummyProduct("Ekspres do kawy DELONGHI", 3300.50),
                new DummyProduct("Hulajnoga elektryczna BLAUPUNKT", 1050.20)
            };
        }

        public List<ICustomer> GetAllCustomers()
        {
            return new List<ICustomer>
            {
                new DummyCustomer("Jan Kowalski", "jan@example.com"),
                new DummyCustomer("Anna Nowak", "anna@example.com")
            };
        }

        public List<IOrder> GetAllOrders()
        {
            ICustomer customer = new DummyCustomer("Jan Kowalski", "jan@example.com");
            return new List<IOrder>
            {
                new DummyOrder(1, customer)
            };
        }
    }
}

