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

        [TestMethod]
        public void Remove_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            DummyCart inventory = new DummyCart(10);
            DummyCustomer customer = new DummyCustomer(Guid.NewGuid(), "Customer1", 1000, inventory);

            bool result = _repository.RemoveCustomer(customer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveById_ShouldRemoveCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            DummyCart inventory = new DummyCart(10);
            DummyCustomer customer = new DummyCustomer(customerId, "Customer1", 1000, inventory);
            _mockContext.Customers[customerId] = customer;

            bool result = _repository.RemoveCustomerById(customerId);

            Assert.IsTrue(result);
            Assert.IsFalse(_mockContext.Customers.ContainsKey(customerId));
        }

        [TestMethod]
        public void RemoveById_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            Guid customerId = Guid.NewGuid();

            bool result = _repository.RemoveCustomerById(customerId);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Update_ShouldUpdateCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            DummyCart inventory = new DummyCart(10);
            DummyCustomer customer = new DummyCustomer(customerId, "Customer1", 1000, inventory);
            _mockContext.Customers[customerId] = customer;

            DummyCustomer updatedCustomer = new DummyCustomer(customerId, "UpdatedCustomer", 1500, inventory);

            bool result = _repository.UpdateCustomer(customerId, updatedCustomer);

            Assert.IsTrue(result);
            Assert.AreEqual(updatedCustomer.Name, _mockContext.Customers[customerId].Name);
        }

        [TestMethod]
        public void Update_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            Guid customerId = Guid.NewGuid();
            DummyCart inventory = new DummyCart(10);
            DummyCustomer updatedCustomer = new DummyCustomer(customerId, "UpdatedCustomer", 1500, inventory);

            bool result = _repository.UpdateCustomer(customerId, updatedCustomer);

            Assert.IsFalse(result);
        }
    }
}
