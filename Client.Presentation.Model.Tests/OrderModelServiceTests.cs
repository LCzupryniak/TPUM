using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;


namespace Client.Presentation.Model.Tests
{
    [TestClass]
    public class OrderModelServiceTests
    {
        private DummyOrderLogic _dummyOrderLogic = null!;
        private DummyCustomerLogic _dummyCustomerLogic = null!;
        private DummyItemLogic _dummyItemLogic = null!;
        private DummyCartLogic _dummyCartLogic = null!;

        private IOrderModelService _orderModelService = null!;

        private Guid _order1Id;
        private Guid _customer1Id, _customer2Id;
        private Guid _inv1Id, _inv2Id;
        private Guid _item1Id, _item2Id, _item3Id;

        private DummyOrderDto _orderDto1 = null!;
        private DummyCustomerDto _customerDto1 = null!;
        private DummyCustomerDto _customerDto2 = null!;
        private DummyCartDto _invDto1 = null!;
        private DummyCartDto _invDto2 = null!;
        private DummyItemDto _itemDto1 = null!;
        private DummyItemDto _itemDto2 = null!;
        private DummyItemDto _itemDto3 = null!; // Item not in order

        [TestInitialize]
        public void TestInitialize()
        {
            _dummyOrderLogic = new DummyOrderLogic();
            _dummyCustomerLogic = new DummyCustomerLogic();
            _dummyItemLogic = new DummyItemLogic();
            _dummyCartLogic = new DummyCartLogic();

            _item1Id = Guid.NewGuid();
            _item2Id = Guid.NewGuid();
            _item3Id = Guid.NewGuid();
            _itemDto1 = new DummyItemDto { Id = _item1Id, Name = "TV", Price = 120, MaintenanceCost = 6 };
            _itemDto2 = new DummyItemDto { Id = _item2Id, Name = "Console", Price = 75, MaintenanceCost = 2 };
            _itemDto3 = new DummyItemDto { Id = _item3Id, Name = "Smartwatch", Price = 40, MaintenanceCost = 1 };
            _dummyItemLogic.Items.Add(_itemDto1.Id, _itemDto1);
            _dummyItemLogic.Items.Add(_itemDto2.Id, _itemDto2);
            _dummyItemLogic.Items.Add(_itemDto3.Id, _itemDto3);


            _inv1Id = Guid.NewGuid();
            _inv2Id = Guid.NewGuid();
            _invDto1 = new DummyCartDto { Id = _inv1Id, Capacity = 10, Items = new List<IProductDataTransferObject>() };
            _invDto2 = new DummyCartDto { Id = _inv2Id, Capacity = 5, Items = new List<IProductDataTransferObject>() };
            _dummyCartLogic.Carts.Add(_invDto1.Id, _invDto1);
            _dummyCartLogic.Carts.Add(_invDto2.Id, _invDto2);

            _customer1Id = Guid.NewGuid();
            _customer2Id = Guid.NewGuid();
            _customerDto1 = new DummyCustomerDto { Id = _customer1Id, Name = "BuyerCustomer", Money = 1000f, Cart = _invDto1 };
            _customerDto2 = new DummyCustomerDto { Id = _customer2Id, Name = "OtherCustomer", Money = 500f, Cart = _invDto2 };
            _dummyCustomerLogic.Customers.Add(_customerDto1.Id, _customerDto1);
            _dummyCustomerLogic.Customers.Add(_customerDto2.Id, _customerDto2);

            _order1Id = Guid.NewGuid();
            _orderDto1 = new DummyOrderDto
            {
                Id = _order1Id,
                Buyer = _customerDto1,
                ItemsToBuy = new List<IProductDataTransferObject> { _itemDto1, _itemDto2 }
            };
            _dummyOrderLogic.Orders.Add(_orderDto1.Id, _orderDto1);


            _orderModelService = ModelFactory.CreateOrderModelService(_dummyOrderLogic, _dummyCustomerLogic, _dummyItemLogic);
        }

        [TestMethod]
        public void GetOrder_ExistingId_ReturnsCorrectMappedOrderModel()
        {
            IOrderModel? order = _orderModelService.GetOrder(_order1Id);

            Assert.IsNotNull(order);
            Assert.AreEqual(_order1Id, order.Id);

            // Verify Buyer
            Assert.IsNotNull(order.Buyer);
            Assert.AreEqual(_customer1Id, order.Buyer.Id);

            // Verify Items
            Assert.IsNotNull(order.ItemsToBuy);
            Assert.AreEqual(2, order.ItemsToBuy.Count());
            Assert.IsTrue(order.ItemsToBuy.Any(i => i.Id == _item1Id));
            Assert.IsTrue(order.ItemsToBuy.Any(i => i.Id == _item2Id));
        }

        [TestMethod]
        public void GetOrder_NonExistingId_ReturnsNull()
        {
            Guid nonExistingId = Guid.NewGuid();

            IOrderModel? order = _orderModelService.GetOrder(nonExistingId);

            Assert.IsNull(order);
        }

        [TestMethod]
        public void AddOrder_ValidData_CallsLogicAddWithCorrectDto()
        {
            Guid newId = Guid.NewGuid();
            Guid buyerId = _customer2Id; // Different customer buys
            List<Guid> itemIdsToBuy = new List<Guid> { _item3Id }; // Buy product

            _orderModelService.AddOrder(newId, buyerId, itemIdsToBuy);

            Assert.IsTrue(_dummyOrderLogic.Orders.ContainsKey(newId));
            IOrderDataTransferObject addedDto = _dummyOrderLogic.Orders[newId];
            Assert.AreEqual(newId, addedDto.Id);

            // Check if Buyer DTO and Item DTOs correctly assigned
            Assert.IsNotNull(addedDto.Buyer);
            Assert.AreEqual(buyerId, addedDto.Buyer.Id);

            Assert.IsNotNull(addedDto.ItemsToBuy);
            Assert.AreEqual(1, addedDto.ItemsToBuy.Count());
            Assert.AreEqual(_item3Id, addedDto.ItemsToBuy.First().Id);
        }

        [TestMethod]
        public void RemoveOrder_ExistingId_CallsLogicRemoveAndReturnsTrue()
        {
            Guid targetId = _order1Id;
            Assert.IsTrue(_dummyOrderLogic.Orders.ContainsKey(targetId));

            bool result = _orderModelService.RemoveOrder(targetId);

            Assert.IsTrue(result);
            Assert.IsFalse(_dummyOrderLogic.Orders.ContainsKey(targetId));
        }

        [TestMethod]
        public void RemoveOrder_NonExistingId_CallsLogicRemoveAndReturnsFalse()
        {
            Guid nonExistingId = Guid.NewGuid();
            int initialCount = _dummyOrderLogic.Orders.Count;

            bool result = _orderModelService.RemoveOrder(nonExistingId);

            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, _dummyOrderLogic.Orders.Count);
        }

    }

}
