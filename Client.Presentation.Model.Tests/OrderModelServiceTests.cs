using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;


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
            _itemDto1 = new DummyItemDto { Id = _item1Id, Name = "Airwrap", Price = 120, MaintenanceCost = 6 };
            _itemDto2 = new DummyItemDto { Id = _item2Id, Name = "Tablet", Price = 75, MaintenanceCost = 2 };
            _itemDto3 = new DummyItemDto { Id = _item3Id, Name = "Microphone", Price = 40, MaintenanceCost = 1 };
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
        public void GetAllOrders_WhenCalled_ReturnsAllMappedOrderModels()
        {
            List<IOrderModel> orders = _orderModelService.GetAllOrders().ToList();

            Assert.IsNotNull(orders);
            Assert.AreEqual(1, orders.Count);

            // Verify mapping for the order
            IOrderModel order1Model = orders.First();
            Assert.AreEqual(_order1Id, order1Model.Id);

            // Verify Buyer mapping
            Assert.IsNotNull(order1Model.Buyer);
            Assert.AreEqual(_customer1Id, order1Model.Buyer.Id);
            Assert.AreEqual("BuyerCustomer", order1Model.Buyer.Name);

            // Verify ItemsToBuy mapping
            Assert.IsNotNull(order1Model.ItemsToBuy);
            Assert.AreEqual(2, order1Model.ItemsToBuy.Count());

            IProductModel? item1Model = order1Model.ItemsToBuy.FirstOrDefault(i => i.Id == _item1Id);
            IProductModel? item2Model = order1Model.ItemsToBuy.FirstOrDefault(i => i.Id == _item2Id);
            Assert.IsNotNull(item1Model);
            Assert.IsNotNull(item2Model);
            Assert.AreEqual("Airwrap", item1Model.Name);
            Assert.AreEqual("Tablet", item2Model.Name);
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

    }

}
