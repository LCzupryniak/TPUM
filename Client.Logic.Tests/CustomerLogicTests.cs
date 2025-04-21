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

        private class TestInventory : ICart
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
            public ICart Inventory { get; set; } = new TestInventory();
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

        [TestMethod]
        public void Add_ShouldAddCustomerToRepository()
        {
            ICustomerDataTransferObject customer = new DummyCustomerDataTransferObject(Guid.NewGuid(), "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));

            _logic.Add(customer);

            ICustomerDataTransferObject? result = _logic.Get(customer.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.Name, result.Name);
        }

        [TestMethod]
        public void RemoveById_ShouldRemoveCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            ICustomerDataTransferObject customer = new DummyCustomerDataTransferObject(customerId, "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            _logic.Add(customer);

            bool result = _logic.RemoveById(customerId);

            Assert.IsTrue(result);
            ICustomerDataTransferObject? removedCustomer = _logic.Get(customerId);
            Assert.IsNull(removedCustomer);
        }

        [TestMethod]
        public void RemoveById_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            Guid customerId = Guid.NewGuid();

            bool result = _logic.RemoveById(customerId);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Remove_ShouldRemoveCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            ICustomerDataTransferObject customer = new DummyCustomerDataTransferObject(customerId, "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            _logic.Add(customer);

            bool result = _logic.Remove(customer);

            Assert.IsTrue(result);
            ICustomerDataTransferObject? removedCustomer = _logic.Get(customerId);
            Assert.IsNull(removedCustomer);
        }

        [TestMethod]
        public void Remove_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            ICustomerDataTransferObject customer = new DummyCustomerDataTransferObject(Guid.NewGuid(), "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));

            bool result = _logic.Remove(customer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Update_ShouldUpdateCustomer_WhenCustomerExists()
        {
            Guid customerId = Guid.NewGuid();
            ICustomerDataTransferObject customer = new DummyCustomerDataTransferObject(customerId, "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            _logic.Add(customer);

            ICustomerDataTransferObject updatedCustomer = new DummyCustomerDataTransferObject(customerId, "UpdatedCustomer", 1500, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));

            bool result = _logic.Update(customerId, updatedCustomer);

            Assert.IsTrue(result);
            ICustomerDataTransferObject? updatedCustomerResult = _logic.Get(customerId);
            Assert.AreEqual("UpdatedCustomer", updatedCustomerResult!.Name);
        }

        [TestMethod]
        public void Update_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            Guid customerId = Guid.NewGuid();
            ICustomerDataTransferObject updatedCustomer = new DummyCustomerDataTransferObject(customerId, "UpdatedCustomer", 1500, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));

            bool result = _logic.Update(customerId, updatedCustomer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PeriodicItemMaintenanceDeduction_ShouldDecreaseMoneyForAllCustomersBasedOnItems()
        {
            var itemDto1_1 = new DummyProductDataTransferObject(Guid.NewGuid(), "TV", 100, 10);
            var itemDto1_2 = new DummyProductDataTransferObject(Guid.NewGuid(), "Laptop", 80, 5);
            var invDto1 = new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject> { itemDto1_1, itemDto1_2 });
            float initialMoney1 = 200f;
            var customerDto1 = new DummyCustomerDataTransferObject(Guid.NewGuid(), "CustomerWithItems", initialMoney1, invDto1);
            float expectedMoney1 = initialMoney1 - itemDto1_1.MaintenanceCost - itemDto1_2.MaintenanceCost;

            var invDto2 = new DummyCartDataTransferObject(Guid.NewGuid(), 5, new List<IProductDataTransferObject>());
            float initialMoney2 = 300f;
            var customerDto2 = new DummyCustomerDataTransferObject(Guid.NewGuid(), "CustomerWithoutItems", initialMoney2, invDto2);
            float expectedMoney2 = initialMoney2;

            var itemDto3_1 = new DummyProductDataTransferObject(Guid.NewGuid(), "Tablet", 500, 20);
            var invDto3 = new DummyCartDataTransferObject(Guid.NewGuid(), 2, new List<IProductDataTransferObject> { itemDto3_1 });
            float initialMoney3 = 50f;
            var customerDto3 = new DummyCustomerDataTransferObject(Guid.NewGuid(), "CustomerPoor", initialMoney3, invDto3);
            float expectedMoney3 = initialMoney3 - itemDto3_1.MaintenanceCost; // 50 - 20

            _logic.Add(customerDto1);
            _logic.Add(customerDto2);
            _logic.Add(customerDto3);

            _logic.PeriodicItemMaintenanceDeduction();

            var resultCustomer1 = _logic.Get(customerDto1.Id);
            var resultCustomer2 = _logic.Get(customerDto2.Id);
            var resultCustomer3 = _logic.Get(customerDto3.Id);

            Assert.IsNotNull(resultCustomer1);
            Assert.AreEqual(expectedMoney1, resultCustomer1.Money, 0.001f, "Customer1 money mismatch after deduction.");

            Assert.IsNotNull(resultCustomer2);
            Assert.AreEqual(expectedMoney2, resultCustomer2.Money, 0.001f, "Customer2 money should not have changed.");

            Assert.IsNotNull(resultCustomer3);
            Assert.AreEqual(expectedMoney3, resultCustomer3.Money, 0.001f, "Customer3 money mismatch after deduction.");
        }
    }
}