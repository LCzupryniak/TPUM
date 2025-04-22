using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Tests
{
    [TestClass]
    public class CustomerModelServiceTests
    {
        private DummyCustomerLogic _dummyCustomerLogic = null!;
        private DummyCartLogic _dummyCartLogic = null!;
        private DummyItemLogic _dummyItemLogic = null!;

        private ICustomerModelService _customerModelService = null!;

        private Guid _customer1Id, _customer2Id;
        private Guid _inv1Id, _inv2Id;
        private Guid _item1Id, _item2Id;

        private DummyCustomerDto _customerDto1 = null!;
        private DummyCustomerDto _customerDto2 = null!;
        private DummyCartDto _invDto1 = null!;
        private DummyCartDto _invDto2 = null!;
        private DummyItemDto _itemDto1 = null!;
        private DummyItemDto _itemDto2 = null!;


        [TestInitialize]
        public void TestInitialize()
        {
            _dummyCustomerLogic = new DummyCustomerLogic();
            _dummyCartLogic = new DummyCartLogic();
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
            _invDto1 = new DummyCartDto { Id = _inv1Id, Capacity = 10, Items = new List<IProductDataTransferObject> { _itemDto1 } };
            _invDto2 = new DummyCartDto { Id = _inv2Id, Capacity = 5, Items = new List<IProductDataTransferObject>() }; // Empty cart
            _dummyCartLogic.Carts.Add(_invDto1.Id, _invDto1);
            _dummyCartLogic.Carts.Add(_invDto2.Id, _invDto2);


            _customer1Id = Guid.NewGuid();
            _customer2Id = Guid.NewGuid();
            _customerDto1 = new DummyCustomerDto { Id = _customer1Id, Name = "Barbara", Money = 500f, Cart = _invDto1 };
            _customerDto2 = new DummyCustomerDto { Id = _customer2Id, Name = "Klemens", Money = 300f, Cart = _invDto2 };
            _dummyCustomerLogic.Customers.Add(_customerDto1.Id, _customerDto1);
            _dummyCustomerLogic.Customers.Add(_customerDto2.Id, _customerDto2);


            _customerModelService = ModelFactory.CreateCustomerModelService(_dummyCustomerLogic, _dummyCartLogic);
        }

        [TestMethod]
        public void GetAllCustomers_WhenCalled_ReturnsAllMappedCustomerModels()
        {
            List<ICustomerModel> customers = _customerModelService.GetAllCustomers().ToList();

            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count);

            ICustomerModel? BarbaraModel = customers.FirstOrDefault(h => h.Id == _customer1Id);
            Assert.IsNotNull(BarbaraModel);
            Assert.AreEqual("Barbara", BarbaraModel.Name);
            Assert.AreEqual(500f, BarbaraModel.Money);
            Assert.IsNotNull(BarbaraModel.Cart);
            Assert.AreEqual(_inv1Id, BarbaraModel.Cart.Id);
            Assert.AreEqual(10, BarbaraModel.Cart.Capacity);
            Assert.AreEqual(1, BarbaraModel.Cart.Items.Count());
            Assert.AreEqual(_item1Id, BarbaraModel.Cart.Items.First().Id);
            Assert.AreEqual("TV", BarbaraModel.Cart.Items.First().Name);
        }

        [TestMethod]
        public void GetCustomer_ExistingId_ReturnsCorrectMappedCustomerModel()
        {
            ICustomerModel? customer = _customerModelService.GetCustomer(_customer1Id);

            Assert.IsNotNull(customer);
            Assert.AreEqual(_customer1Id, customer.Id);
            Assert.AreEqual("Barbara", customer.Name);
            Assert.AreEqual(500f, customer.Money);
            Assert.IsNotNull(customer.Cart);
            Assert.AreEqual(_inv1Id, customer.Cart.Id);
            Assert.AreEqual(1, customer.Cart.Items.Count());
        }

        [TestMethod]
        public void GetCustomer_NonExistingId_ReturnsNull()
        {
            Guid nonExistingId = Guid.NewGuid();

            ICustomerModel? customer = _customerModelService.GetCustomer(nonExistingId);

            Assert.IsNull(customer);
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
        public void TriggerPeriodicItemMaintenanceDeduction_WhenCalled_CallsLogicMethod()
        {
            int initialCallCount = _dummyCustomerLogic.PeriodicDeductionCallCount;

            _customerModelService.TriggerPeriodicItemMaintenanceDeduction();

            Assert.AreEqual(initialCallCount + 1, _dummyCustomerLogic.PeriodicDeductionCallCount);
        }
    }
}