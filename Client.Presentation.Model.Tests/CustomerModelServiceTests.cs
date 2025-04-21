using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Tests
{
    [TestClass]
    public class CustomerModelServiceTests
    {
        private DummyCustomerLogic _dummyCustomerLogic = null!;
        private DummyInventoryLogic _dummyInventoryLogic = null!;
        private DummyItemLogic _dummyItemLogic = null!;

        private ICustomerModelService _customerModelService = null!;

        private Guid _customer1Id, _customer2Id;
        private Guid _inv1Id, _inv2Id;
        private Guid _item1Id, _item2Id;

        private DummyCustomerDto _customerDto1 = null!;
        private DummyCustomerDto _customerDto2 = null!;
        private DummyInventoryDto _invDto1 = null!;
        private DummyInventoryDto _invDto2 = null!;
        private DummyItemDto _itemDto1 = null!;
        private DummyItemDto _itemDto2 = null!;


        [TestInitialize]
        public void TestInitialize()
        {
            _dummyCustomerLogic = new DummyCustomerLogic();
            _dummyInventoryLogic = new DummyInventoryLogic();
            _dummyItemLogic = new DummyItemLogic();

            // Create Test DTOs
            _item1Id = Guid.NewGuid();
            _item2Id = Guid.NewGuid();
            _itemDto1 = new DummyItemDto { Id = _item1Id, Name = "TV", Price = 100, MaintenanceCost = 5 };
            _itemDto2 = new DummyItemDto { Id = _item2Id, Name = "Laptop", Price = 80, MaintenanceCost = 3 };
            _dummyItemLogic.Items.Add(_itemDto1.Id, _itemDto1);
            _dummyItemLogic.Items.Add(_itemDto2.Id, _itemDto2);


            _inv1Id = Guid.NewGuid();
            _inv2Id = Guid.NewGuid();
            _invDto1 = new DummyInventoryDto { Id = _inv1Id, Capacity = 10, Items = new List<IProductDataTransferObject> { _itemDto1 } };
            _invDto2 = new DummyInventoryDto { Id = _inv2Id, Capacity = 5, Items = new List<IProductDataTransferObject>() }; // Empty inventory
            _dummyInventoryLogic.Inventories.Add(_invDto1.Id, _invDto1);
            _dummyInventoryLogic.Inventories.Add(_invDto2.Id, _invDto2);


            _customer1Id = Guid.NewGuid();
            _customer2Id = Guid.NewGuid();
            _customerDto1 = new DummyCustomerDto { Id = _customer1Id, Name = "Barbara", Money = 500f, Inventory = _invDto1 };
            _customerDto2 = new DummyCustomerDto { Id = _customer2Id, Name = "Klemens", Money = 300f, Inventory = _invDto2 };
            _dummyCustomerLogic.Customers.Add(_customerDto1.Id, _customerDto1);
            _dummyCustomerLogic.Customers.Add(_customerDto2.Id, _customerDto2);


            _customerModelService = ModelFactory.CreateCustomerModelService(_dummyCustomerLogic, _dummyInventoryLogic);
        }

        [TestMethod]
        public void GetAllCustomers_WhenCalled_ReturnsAllMappedCustomerModels()
        {
            List<ICustomerModel> customers = _customerModelService.GetAllCustomers().ToList();

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count);

            ICustomerModel? BobModel = customers.FirstOrDefault(h => h.Id == _customer1Id);
            Assert.IsNotNull(BobModel);
            Assert.AreEqual("Barbara", BobModel.Name);
            Assert.AreEqual(500f, BobModel.Money);
            Assert.IsNotNull(BobModel.Inventory);
            Assert.AreEqual(_inv1Id, BobModel.Inventory.Id);
            Assert.AreEqual(10, BobModel.Inventory.Capacity);
            Assert.AreEqual(1, BobModel.Inventory.Items.Count());
            Assert.AreEqual(_item1Id, BobModel.Inventory.Items.First().Id);
            Assert.AreEqual("TV", BobModel.Inventory.Items.First().Name);
        }

        [TestMethod]
        public void GetCustomer_ExistingId_ReturnsCorrectMappedCustomerModel()
        {
            ICustomerModel? customer = _customerModelService.GetCustomer(_customer1Id);

            Assert.IsNotNull(customer);
            Assert.AreEqual(_customer1Id, customer.Id);
            Assert.AreEqual("Barbara", customer.Name);
            Assert.AreEqual(500f, customer.Money);
            Assert.IsNotNull(customer.Inventory);
            Assert.AreEqual(_inv1Id, customer.Inventory.Id);
            Assert.AreEqual(1, customer.Inventory.Items.Count());
        }

        [TestMethod]
        public void GetCustomer_NonExistingId_ReturnsNull()
        {
            Guid nonExistingId = Guid.NewGuid();

            ICustomerModel? customer = _customerModelService.GetCustomer(nonExistingId);

            Assert.IsNull(customer);
        }

        [TestMethod]
        public void AddCustomer_ValidData_CallsLogicAdd()
        {
            Guid newId = Guid.NewGuid();
            string newName = "Newbie";
            float newMoney = 10f;
            Guid newInventoryId = _inv2Id;

            _customerModelService.AddCustomer(newId, newName, newMoney, newInventoryId);

            Assert.IsTrue(_dummyCustomerLogic.Customers.ContainsKey(newId));
            ICustomerDataTransferObject addedDto = _dummyCustomerLogic.Customers[newId];
            Assert.AreEqual(newName, addedDto.Name);
            Assert.AreEqual(newMoney, addedDto.Money);
            Assert.AreEqual(newInventoryId, addedDto.Inventory.Id);
        }

        [TestMethod]
        public void RemoveCustomer_ExistingId_CallsLogicRemoveAndReturnsTrue()
        {
            Guid targetId = _customer1Id;
            Assert.IsTrue(_dummyCustomerLogic.Customers.ContainsKey(targetId));

            bool result = _customerModelService.RemoveCustomer(targetId);

            Assert.IsTrue(result);
            Assert.IsFalse(_dummyCustomerLogic.Customers.ContainsKey(targetId));
        }

        [TestMethod]
        public void RemoveCustomer_NonExistingId_CallsLogicRemoveAndReturnsFalse()
        {
            Guid nonExistingId = Guid.NewGuid();
            int initialCount = _dummyCustomerLogic.Customers.Count;

            bool result = _customerModelService.RemoveCustomer(nonExistingId);

            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, _dummyCustomerLogic.Customers.Count);
        }


        [TestMethod]
        public void UpdateCustomer_ExistingId_CallsLogicUpdateAndReturnsTrue()
        {
            Guid targetId = _customer1Id;
            string updatedName = "Barbara Updated";
            float updatedMoney = 550f;
            Guid updatedInventoryId = _inv2Id; // Change inventory

            // Find the inventory DTO to pass to the updated customer DTO
            _dummyInventoryLogic.Inventories.TryGetValue(updatedInventoryId, out IInventoryDataTransferObject? updatedInventoryDto);
            Assert.IsNotNull(updatedInventoryDto);

            bool result = _customerModelService.UpdateCustomer(targetId, updatedName, updatedMoney, updatedInventoryId);

            Assert.IsTrue(result);
            Assert.IsTrue(_dummyCustomerLogic.Customers.ContainsKey(targetId));
            ICustomerDataTransferObject updatedDto = _dummyCustomerLogic.Customers[targetId];
            Assert.AreEqual(updatedName, updatedDto.Name);
            Assert.AreEqual(updatedMoney, updatedDto.Money);
            Assert.IsNotNull(updatedDto.Inventory);
            Assert.AreEqual(updatedInventoryId, updatedDto.Inventory.Id);
        }

        [TestMethod]
        public void UpdateCustomer_NonExistingId_CallsLogicUpdateAndReturnsFalse()
        {
            Guid nonExistingId = Guid.NewGuid();
            int initialCount = _dummyCustomerLogic.Customers.Count;

            // Pass a valid inventory ID even though the customer doesn't exist
            bool result = _customerModelService.UpdateCustomer(nonExistingId, "Doesn't Exist", 0f, _inv1Id);

            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, _dummyCustomerLogic.Customers.Count);
        }

        [TestMethod]
        public void TriggerPeriodicItemMaintenanceDeduction_WhenCalled_CallsLogicMethod()
        {
            int initialCallCount = _dummyCustomerLogic.PeriodicDeductionCallCount;

            _customerModelService.TriggerPeriodicItemMaintenanceDeduction();

            Assert.AreEqual(initialCallCount + 1, _dummyCustomerLogic.PeriodicDeductionCallCount);
        }
    }
}