using ClientServer.Shared.Logic.API;
using Client.Logic.API;
using ClientServer.Shared.Data.API;

namespace Client.Logic.Tests
{
    [TestClass]
    public class CartLogicTests
    {
        private ICartLogic _logic = default!;

        [TestInitialize]
        public void SetUp()
        {
            _logic = LogicFactory.CreateCartLogic(new DummyDataRepository());
        }

        private class TestItem : IProduct
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = "ItemInCart";
            public int Price { get; set; } = 5;
            public int MaintenanceCost { get; set; } = 0;
        }

        private class TestCart : ICart
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public int Capacity { get; set; } = 10;
            public List<IProduct> Items { get; set; } = new List<IProduct>();
        }

        [TestMethod]
        public void Add_ShouldAddCartToRepository()
        {
            DummyCartDataTransferObject cart = new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>());
            _logic.Add(cart);
            ICartDataTransferObject? retrieved = _logic.Get(cart.Id);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual(cart.Capacity, retrieved.Capacity);
        }

        [TestMethod]
        public void Get_ShouldReturnCart_WhenExists()
        {
            Guid cartId = Guid.NewGuid();
            DummyCartDataTransferObject cart = new DummyCartDataTransferObject(cartId, 10, new List<IProductDataTransferObject>());
            _logic.Add(cart);
            ICartDataTransferObject? retrieved = _logic.Get(cartId);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual(cart.Capacity, retrieved.Capacity);
        }

        [TestMethod]
        public void RemoveById_ShouldRemoveCart_WhenExists()
        {
            Guid cartId = Guid.NewGuid();
            DummyCartDataTransferObject cart = new DummyCartDataTransferObject(cartId, 10, new List<IProductDataTransferObject>());
            _logic.Add(cart);
            bool removed = _logic.RemoveById(cartId);
            Assert.IsTrue(removed);
            Assert.IsNull(_logic.Get(cartId));
        }

        [TestMethod]
        public void RemoveById_ShouldReturnFalse_WhenCartDoesNotExist()
        {
            Guid cartId = Guid.NewGuid();
            bool removed = _logic.RemoveById(cartId);
            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Remove_ShouldRemoveCart_WhenExists()
        {
            DummyCartDataTransferObject cart = new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>());
            _logic.Add(cart);
            bool removed = _logic.Remove(cart);
            Assert.IsTrue(removed);
            Assert.IsNull(_logic.Get(cart.Id));
        }

        [TestMethod]
        public void Update_ShouldReturnFalse_WhenCartDoesNotExist()
        {
            DummyCartDataTransferObject cart = new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>());
            bool updated = _logic.Update(cart.Id, cart);
            Assert.IsFalse(updated);
        }
    }
}
