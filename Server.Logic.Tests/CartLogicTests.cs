using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;
using Server.Logic.API;

namespace Server.Logic.Tests
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
        public void Get_ShouldReturnNull_WhenCartDoesNotExist()
        {
            Guid cartId = Guid.NewGuid();
            ICartDataTransferObject? result = _logic.Get(cartId);
            Assert.IsNull(result);
        }
    }
}
