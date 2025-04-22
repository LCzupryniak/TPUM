using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;
using Server.Logic.API;

namespace Server.Logic.Tests
{
    [TestClass]
    public class OrderLogicTests
    {
        private IOrderLogic _logic = default!;

        [TestInitialize]
        public void SetUp()
        {
            _logic = LogicFactory.CreateOrderLogic(new DummyDataRepository());
        }

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
            public string Name { get; set; } = "Test Customer Buyer";
            public float Money { get; set; } = 100f;
            public ICart Cart { get; set; } = new TestCart();
        }

        private class TestOrder : IOrder
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public ICustomer Buyer { get; set; } = new TestCustomer();
            public IEnumerable<IProduct> ItemsToBuy { get; set; } = new List<IProduct>();
        }


        [TestMethod]
        public void GetAll_ShouldReturnAllOrders()
        {
            ICustomerDataTransferObject buyer = new DummyCustomerDataTransferObject(Guid.NewGuid(), "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            IProductDataTransferObject item1 = new DummyProductDataTransferObject(Guid.NewGuid(), "Item1", 100, 10);
            IProductDataTransferObject item2 = new DummyProductDataTransferObject(Guid.NewGuid(), "Item2", 200, 20);
            IOrderDataTransferObject order1 = new DummyOrderDataTransferObject(Guid.NewGuid(), buyer, new List<IProductDataTransferObject> { item1 });
            IOrderDataTransferObject order2 = new DummyOrderDataTransferObject(Guid.NewGuid(), buyer, new List<IProductDataTransferObject> { item2 });

            _logic.Add(order1);
            _logic.Add(order2);

            IEnumerable<IOrderDataTransferObject> orders = _logic.GetAll();

            Assert.IsNotNull(orders);
            Assert.AreEqual(2, orders.Count());
            Assert.IsTrue(orders.Any(o => o.Id == order1.Id));
            Assert.IsTrue(orders.Any(o => o.Id == order2.Id));
        }

        [TestMethod]
        public void Get_ShouldReturnOrder_WhenOrderExists()
        {
            Guid orderId = Guid.NewGuid();
            ICustomerDataTransferObject buyer = new DummyCustomerDataTransferObject(Guid.NewGuid(), "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            IProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Item1", 100, 10);
            IOrderDataTransferObject order = new DummyOrderDataTransferObject(orderId, buyer, new List<IProductDataTransferObject> { item });

            _logic.Add(order);

            IOrderDataTransferObject? result = _logic.Get(orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(orderId, result.Id);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            Guid orderId = Guid.NewGuid();

            IOrderDataTransferObject? result = _logic.Get(orderId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Add_ShouldAddOrderToRepository()
        {
            Guid orderId = Guid.NewGuid();
            ICustomerDataTransferObject buyer = new DummyCustomerDataTransferObject(Guid.NewGuid(), "Customer1", 1000, new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>()));
            IProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Item1", 100, 10);
            IOrderDataTransferObject order = new DummyOrderDataTransferObject(orderId, buyer, new List<IProductDataTransferObject> { item });

            _logic.Add(order);

            IOrderDataTransferObject? result = _logic.Get(orderId);
            Assert.IsNotNull(result);
            Assert.AreEqual(orderId, result.Id);
        }
    }
}
