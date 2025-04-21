using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;
using Server.Logic.API;

namespace Server.Logic.Tests
{
    [TestClass]
    public class ProductLogicTests
    {
        private IProductLogic _logic = default!;

        [TestInitialize]
        public void SetUp()
        {
            _logic = LogicFactory.CreateItemLogic(new DummyDataRepository());
        }

        private class TestItem : IProduct
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = "Default Test Item";
            public int Price { get; set; } = 10;
            public int MaintenanceCost { get; set; } = 1;
        }

        [TestMethod]
        public void Add_ShouldAddItemToLogic()
        {
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "TV", 100, 5);
            _logic.Add(item);

            IProductDataTransferObject? result = _logic.Get(item.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(item.Name, result.Name);
        }

        [TestMethod]
        public void Get_ShouldReturnItem_WhenItemExists()
        {
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Mixer", 150, 8);
            _logic.Add(item);

            IProductDataTransferObject? result = _logic.Get(item.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(item.Price, result.Price);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenItemDoesNotExist()
        {
            IProductDataTransferObject? result = _logic.Get(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RemoveById_ShouldRemoveItem_WhenItemExists()
        {
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Smartwatch", 120, 6);
            _logic.Add(item);
            bool removed = _logic.RemoveById(item.Id);

            Assert.IsTrue(removed);
            Assert.IsNull(_logic.Get(item.Id));
        }

        [TestMethod]
        public void RemoveById_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            bool removed = _logic.RemoveById(Guid.NewGuid());
            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Remove_ShouldRemoveItem_WhenItemExists()
        {
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "iPhone", 50, 2);
            _logic.Add(item);
            bool removed = _logic.Remove(item);

            Assert.IsTrue(removed);
            Assert.IsNull(_logic.Get(item.Id));
        }

        [TestMethod]
        public void Remove_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Airwrap", 200, 10);
            bool removed = _logic.Remove(item);
            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Update_ShouldUpdateItem_WhenItemExists()
        {
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Tablet", 180, 9);
            _logic.Add(item);
            DummyProductDataTransferObject updatedItem = new DummyProductDataTransferObject(item.Id, "Updated Tablet", 200, 12);
            bool updated = _logic.Update(item.Id, updatedItem);

            Assert.IsTrue(updated);
            IProductDataTransferObject? result = _logic.Get(item.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Tablet", result.Name);
        }

        [TestMethod]
        public void Update_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Laptop", 220, 15);
            bool updated = _logic.Update(item.Id, item);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllItems()
        {
            DummyProductDataTransferObject item1 = new DummyProductDataTransferObject(Guid.NewGuid(), "Camera", 120, 6);
            DummyProductDataTransferObject item2 = new DummyProductDataTransferObject(Guid.NewGuid(), "RobotCleaner", 140, 7);
            _logic.Add(item1);
            _logic.Add(item2);

            IEnumerable<IProductDataTransferObject> items = _logic.GetAll();
            Assert.AreEqual(2, items.Count());
        }
    }
}