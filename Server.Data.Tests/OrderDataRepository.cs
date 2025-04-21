using Server.Data.API;
using ClientServer.Shared.Data.API;

namespace Server.Data.Tests
{
    [TestClass]
    public class OrderDataRepositoryTests : DataRepositoryTestBase
    {
        [TestMethod]
        public void Add_ShouldAddOrderToRepository()
        {
            DummyCustomer customer = new DummyCustomer("Customer1", 1000, new DummyCart(10));
            List<IProduct> itemsToBuy = new List<IProduct> { new DummyProduct("TV", 100, 5) };
            DummyOrder order = new DummyOrder(customer, itemsToBuy);

            _repository.AddOrder(order);

            Assert.IsTrue(_mockContext.Orders.ContainsKey(order.Id));
            Assert.AreEqual(customer.Name, _mockContext.Orders[order.Id].Buyer.Name);
        }

        [TestMethod]
        public void Get_ShouldReturnOrder_WhenOrderExists()
        {
            Guid orderId = Guid.NewGuid();
            DummyCustomer customer = new DummyCustomer("Customer1", 1000, new DummyCart(10));
            List<IProduct> itemsToBuy = new List<IProduct> { new DummyProduct("TV", 100, 5) };
            DummyOrder order = new DummyOrder(orderId, customer, itemsToBuy);
            _mockContext.Orders[orderId] = order;

            IOrder? result = _repository.GetOrder(orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(orderId, result.Id);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            Guid orderId = Guid.NewGuid();

            IOrder? result = _repository.GetOrder(orderId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_ShouldRemoveOrder_WhenOrderExists()
        {
            Guid orderId = Guid.NewGuid();
            DummyCustomer customer = new DummyCustomer("Customer1", 1000, new DummyCart(10));
            List<IProduct> itemsToBuy = new List<IProduct> { new DummyProduct("TV", 100, 5) };
            DummyOrder order = new DummyOrder(orderId, customer, itemsToBuy);
            _mockContext.Orders[orderId] = order;

            bool result = _repository.RemoveOrder(order);

            Assert.IsTrue(result);
            Assert.IsFalse(_mockContext.Orders.ContainsKey(orderId));
        }
    }
}
