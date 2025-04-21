using ClientServer.Shared.Data.API;
using Server.Data.API;

namespace Server.Data.Tests
{
    [TestClass]
    public class CustomerDataRepositoryTests : DataRepositoryTestBase
    {

        [TestMethod]
        public void Add_ShouldAddCustomerToRepository()
        {
            DummyCart inventory = new DummyCart(10);
            DummyCustomer customer = new DummyCustomer("Customer1", 1000, inventory);

            _repository.AddCustomer(customer);

            Assert.IsTrue(_mockContext.Customers.ContainsKey(customer.Id));
            Assert.AreEqual(customer.Name, _mockContext.Customers[customer.Id].Name);
        }

        [TestMethod]
        public void Get_ShouldReturnCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            DummyCart inventory = new DummyCart(10);
            DummyCustomer customer = new DummyCustomer(customerId, "Customer1", 1000, inventory);

            _mockContext.Customers[customerId] = customer;

            ICustomer? result = _repository.GetCustomer(customerId);

            Assert.IsNotNull(result);
            Assert.AreEqual(customer.Name, result.Name);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            Guid customerId = Guid.NewGuid();

            ICustomer? result = _repository.GetCustomer(customerId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_ShouldRemoveCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            DummyCart inventory = new DummyCart(10);
            DummyCustomer customer = new DummyCustomer(customerId, "Customer1", 1000, inventory);
            _mockContext.Customers[customerId] = customer;

            bool result = _repository.RemoveCustomer(customer);

            Assert.IsTrue(result);
            Assert.IsFalse(_mockContext.Customers.ContainsKey(customerId));
        }
    }
}
