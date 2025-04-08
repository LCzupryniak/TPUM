using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace Data.Implementation
{
    internal class DataRepository : IDataRepository
    {
        public List<IProduct> GetAllProducts()
        {
            return new List<IProduct>
            {
                new Product("Telewizor SAMSUNG QLED 4K ", 2999.99),
                new Product("Ekspres do kawy DELONGHI", 3300.50),
                new Product("Hulajnoga elektryczna BLAUPUNKT", 1050.20)
            };
        }

        public List<ICustomer> GetAllCustomers()
        {
            return new List<ICustomer>
            {
                new Customer("Jan Kowalski", "jan@example.com"),
                new Customer("Anna Nowak", "anna@example.com")
            };
        }

        public List<IOrder> GetAllOrders()
        {
            ICustomer customer = new Customer("Jan Kowalski", "jan@example.com");
            return new List<IOrder>
            {
                new Order(1, customer)
            };
        }
    }
}
