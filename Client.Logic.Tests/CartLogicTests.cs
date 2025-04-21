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
            _logic = LogicFactory.CreateInventoryLogic(new DummyDataRepository());
        }

        private class TestItem : IProduct
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = "ItemInInventory";
            public int Price { get; set; } = 5;
            public int MaintenanceCost { get; set; } = 0;
        }

        private class TestInventory : ICart
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public int Capacity { get; set; } = 10;
            public List<IProduct> Items { get; set; } = new List<IProduct>();
        }

        [TestMethod]
        public void Add_ShouldAddInventoryToRepository()
        {
            DummyCartDataTransferObject inventory = new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>());
            _logic.Add(inventory);
            IInventoryDataTransferObject? retrieved = _logic.Get(inventory.Id);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual(inventory.Capacity, retrieved.Capacity);
        }

        [TestMethod]
        public void Get_ShouldReturnInventory_WhenExists()
        {
            Guid inventoryId = Guid.NewGuid();
            DummyCartDataTransferObject inventory = new DummyCartDataTransferObject(inventoryId, 10, new List<IProductDataTransferObject>());
            _logic.Add(inventory);
            IInventoryDataTransferObject? retrieved = _logic.Get(inventoryId);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual(inventory.Capacity, retrieved.Capacity);
        }

        [TestMethod]
        public void RemoveById_ShouldRemoveInventory_WhenExists()
        {
            Guid inventoryId = Guid.NewGuid();
            DummyCartDataTransferObject inventory = new DummyCartDataTransferObject(inventoryId, 10, new List<IProductDataTransferObject>());
            _logic.Add(inventory);
            bool removed = _logic.RemoveById(inventoryId);
            Assert.IsTrue(removed);
            Assert.IsNull(_logic.Get(inventoryId));
        }

        [TestMethod]
        public void RemoveById_ShouldReturnFalse_WhenInventoryDoesNotExist()
        {
            Guid inventoryId = Guid.NewGuid();
            bool removed = _logic.RemoveById(inventoryId);
            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Remove_ShouldRemoveInventory_WhenExists()
        {
            DummyCartDataTransferObject inventory = new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>());
            _logic.Add(inventory);
            bool removed = _logic.Remove(inventory);
            Assert.IsTrue(removed);
            Assert.IsNull(_logic.Get(inventory.Id));
        }

        [TestMethod]
        public void Update_ShouldReturnFalse_WhenInventoryDoesNotExist()
        {
            DummyCartDataTransferObject inventory = new DummyCartDataTransferObject(Guid.NewGuid(), 10, new List<IProductDataTransferObject>());
            bool updated = _logic.Update(inventory.Id, inventory);
            Assert.IsFalse(updated);
        }
    }
}
