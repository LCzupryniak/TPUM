using Client.Logic.API;
using ClientServer.Shared.Logic.API;
using ClientServer.Shared.Data.API;

namespace Client.Logic.Tests
{
    [TestClass]
    public class CustomerLogicTests
    {
        private ICustomerLogic _logic = default!;

        [TestInitialize]
        public void SetUp()
        {
            _logic = LogicFactory.CreateCustomerLogic(new DummyDataRepository());
        }

        // Data Layer classes for Mapping tests
        private class TestItem : IProduct
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = "Test Item";
            public int Price { get; set; } = 0;
            public int MaintenanceCost { get; set; } = 0;
        }

        private class TestCart : ICart
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public int Capacity { get; set; } = 10;
            public List<IProduct> Items { get; set; } = new List<IProduct>();
        }

        private class TestCustomer : ICustomer
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = "Test Customer";
            public float Money { get; set; } = 100f;
            public ICart Cart { get; set; } = new TestCart();
        }



        [TestMethod]
        public void GetAll_ShouldReturnAllCustomers()
        {
            ICustomerDataTransferObject customer1 = new DummyCustomerDataTransferObject(Guid.NewGuid(), "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            ICustomerDataTransferObject customer2 = new DummyCustomerDataTransferObject(Guid.NewGuid(), "Customer2", 1500, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));

            _logic.Add(customer1);
            _logic.Add(customer2);

            IEnumerable<ICustomerDataTransferObject> customers = _logic.GetAll();

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count());
            Assert.IsTrue(customers.Any(h => h.Name == "Customer1"));
            Assert.IsTrue(customers.Any(h => h.Name == "Customer2"));
        }

        [TestMethod]
        public void Get_ShouldReturnCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            ICustomerDataTransferObject customer = new DummyCustomerDataTransferObject(customerId, "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            _logic.Add(customer);

            ICustomerDataTransferObject? result = _logic.Get(customerId);

            Assert.IsNotNull(result);
            Assert.AreEqual(customerId, result.Id);
            Assert.AreEqual("Customer1", result.Name);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            Guid customerId = Guid.NewGuid();

            ICustomerDataTransferObject? result = _logic.Get(customerId);

            Assert.IsNull(result);
        }

    }
}