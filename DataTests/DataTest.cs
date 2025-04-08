using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Implementation;
using Data.API;

namespace DataTests
{
    [TestClass]
    public class DataTest
    {
        private IDataRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Dependency Injection poprzez inicjalizację przed każdym testem
            _repository = new DummyDataRepository();
        }

        [TestMethod]
        public void GetAllProducts_ShouldReturnThreeProducts()
        {
            // Arrange
            int expectedCount = 3;

            // Act
            List<IProduct> products = _repository.GetAllProducts();

            // Assert
            Assert.IsNotNull(products);
            Assert.AreEqual(expectedCount, products.Count);
        }

        [TestMethod]
        public void GetAllProducts_ShouldReturnProductsWithCorrectNames()
        {
            // Arrange
            string[] expectedNames = {
                "Telewizor SAMSUNG QLED 4K ",
                "Ekspres do kawy DELONGHI",
                "Hulajnoga elektryczna BLAUPUNKT"
            };

            // Act
            List<IProduct> products = _repository.GetAllProducts();

            // Assert
            Assert.IsNotNull(products);
            for (int i = 0; i < expectedNames.Length; i++)
            {
                Assert.AreEqual(expectedNames[i], products[i].Name);
            }
        }

        [TestMethod]
        public void GetAllProducts_ShouldReturnProductsWithCorrectPrices()
        {
            // Arrange
            double[] expectedPrices = { 2999.99, 3300.50, 1050.20 };

            // Act
            List<IProduct> products = _repository.GetAllProducts();

            // Assert
            Assert.IsNotNull(products);
            for (int i = 0; i < expectedPrices.Length; i++)
            {
                Assert.AreEqual(expectedPrices[i], products[i].Price);
            }
        }

        [TestMethod]
        public void GetAllCustomers_ShouldReturnTwoCustomers()
        {
            // Arrange
            int expectedCount = 2;

            // Act
            List<ICustomer> customers = _repository.GetAllCustomers();

            // Assert
            Assert.IsNotNull(customers);
            Assert.AreEqual(expectedCount, customers.Count);
        }

        [TestMethod]
        public void GetAllCustomers_ShouldReturnCustomersWithCorrectNames()
        {
            // Arrange
            string[] expectedNames = { "Jan Kowalski", "Anna Nowak" };

            // Act
            List<ICustomer> customers = _repository.GetAllCustomers();

            // Assert
            Assert.IsNotNull(customers);
            for (int i = 0; i < expectedNames.Length; i++)
            {
                Assert.AreEqual(expectedNames[i], customers[i].Name);
            }
        }

        [TestMethod]
        public void GetAllCustomers_ShouldReturnCustomersWithCorrectEmails()
        {
            // Arrange
            string[] expectedEmails = { "jan@example.com", "anna@example.com" };

            // Act
            List<ICustomer> customers = _repository.GetAllCustomers();

            // Assert
            Assert.IsNotNull(customers);
            for (int i = 0; i < expectedEmails.Length; i++)
            {
                Assert.AreEqual(expectedEmails[i], customers[i].Email);
            }
        }

        [TestMethod]
        public void GetAllOrders_ShouldReturnOneOrder()
        {
            // Arrange
            int expectedCount = 1;

            // Act
            List<IOrder> orders = _repository.GetAllOrders();

            // Assert
            Assert.IsNotNull(orders);
            Assert.AreEqual(expectedCount, orders.Count);
        }

        [TestMethod]
        public void GetAllOrders_ShouldReturnOrderWithCorrectId()
        {
            // Arrange
            int expectedId = 1;

            // Act
            List<IOrder> orders = _repository.GetAllOrders();

            // Assert
            Assert.IsNotNull(orders);
            Assert.AreEqual(expectedId, orders[0].Id);
        }

        [TestMethod]
        public void GetAllOrders_ShouldReturnOrderWithCorrectCustomer()
        {
            // Arrange
            string expectedCustomerName = "Jan Kowalski";
            string expectedCustomerEmail = "jan@example.com";

            // Act
            List<IOrder> orders = _repository.GetAllOrders();

            // Assert
            Assert.IsNotNull(orders);
            Assert.IsNotNull(orders[0].Customer);
            Assert.AreEqual(expectedCustomerName, orders[0].Customer.Name);
            Assert.AreEqual(expectedCustomerEmail, orders[0].Customer.Email);
        }
    }
}
