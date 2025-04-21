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
        public void Get_ShouldReturnNull_WhenInventoryDoesNotExist()
        {
            Guid inventoryId = Guid.NewGuid();
            IInventoryDataTransferObject? result = _logic.Get(inventoryId);
            Assert.IsNull(result);
        }
    }
}
