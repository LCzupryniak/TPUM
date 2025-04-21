using ClientServer.Shared.Data.API;

namespace Client.Data.Tests
{
    [TestClass]
    public class CartDataRepositoryTests : DataRepositoryTestBase
    {
        [TestMethod]
        public void Add_ShouldAddInventoryToRepository()
        {
            DummyCart inventory = new DummyCart(10);

            _repository.AddInventory(inventory);

            Assert.IsTrue(_mockContext.Inventories.ContainsKey(inventory.Id));
            Assert.AreEqual(inventory.Capacity, _mockContext.Inventories[inventory.Id].Capacity);
        }

        [TestMethod]
        public void Get_ShouldReturnInventory_WhenInventoryExists()
        {
            Guid inventoryId = Guid.NewGuid();
            DummyCart inventory = new DummyCart(inventoryId, 10);
            _mockContext.Inventories[inventoryId] = inventory;

            ICart? result = _repository.GetInventory(inventoryId);

            Assert.IsNotNull(result);
            Assert.AreEqual(inventory.Capacity, result.Capacity);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenInventoryDoesNotExist()
        {
            Guid inventoryId = Guid.NewGuid();

            ICart? result = _repository.GetInventory(inventoryId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_ShouldRemoveInventory_WhenInventoryExists()
        {
            Guid inventoryId = Guid.NewGuid();
            DummyCart inventory = new DummyCart(inventoryId, 10);
            _mockContext.Inventories[inventoryId] = inventory;

            bool result = _repository.RemoveInventory(inventory);

            Assert.IsTrue(result);
            Assert.IsFalse(_mockContext.Inventories.ContainsKey(inventoryId));
        }

        [TestMethod]
        public void Remove_ShouldReturnFalse_WhenInventoryDoesNotExist()
        {
            DummyCart inventory = new DummyCart(Guid.NewGuid(), 10);

            bool result = _repository.RemoveInventory(inventory);

            Assert.IsFalse(result);
        }

    }
}