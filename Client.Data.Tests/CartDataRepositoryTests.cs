using Client.ObjectModels.Data.API;

namespace Client.Data.Tests
{
    [TestClass]
    public class CartDataRepositoryTests : DataRepositoryTestBase
    {
        [TestMethod]
        public void Add_ShouldAddCartToRepository()
        {
            DummyCart cart = new DummyCart(10);

            _repository.AddCart(cart);

            Assert.IsTrue(_mockContext.Carts.ContainsKey(cart.Id));
            Assert.AreEqual(cart.Capacity, _mockContext.Carts[cart.Id].Capacity);
        }

        [TestMethod]
        public void Get_ShouldReturnCart_WhenCartExists()
        {
            Guid cartId = Guid.NewGuid();
            DummyCart cart = new DummyCart(cartId, 10);
            _mockContext.Carts[cartId] = cart;

            ICart? result = _repository.GetCart(cartId);

            Assert.IsNotNull(result);
            Assert.AreEqual(cart.Capacity, result.Capacity);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenCartDoesNotExist()
        {
            Guid cartId = Guid.NewGuid();

            ICart? result = _repository.GetCart(cartId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_ShouldRemoveCart_WhenCartExists()
        {
            Guid cartId = Guid.NewGuid();
            DummyCart cart = new DummyCart(cartId, 10);
            _mockContext.Carts[cartId] = cart;

            bool result = _repository.RemoveCart(cart);

            Assert.IsTrue(result);
            Assert.IsFalse(_mockContext.Carts.ContainsKey(cartId));
        }

        [TestMethod]
        public void Remove_ShouldReturnFalse_WhenCartDoesNotExist()
        {
            DummyCart cart = new DummyCart(Guid.NewGuid(), 10);

            bool result = _repository.RemoveCart(cart);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveById_ShouldRemoveCart_WhenCartExists()
        {
            Guid cartId = Guid.NewGuid();
            DummyCart cart = new DummyCart(cartId, 10);
            _mockContext.Carts[cartId] = cart;

            bool result = _repository.RemoveCartById(cartId);

            Assert.IsTrue(result);
            Assert.IsFalse(_mockContext.Carts.ContainsKey(cartId));
        }
    }
}