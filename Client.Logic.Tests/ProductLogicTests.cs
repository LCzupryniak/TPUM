using Client.Logic.API;
using Client.ObjectModels.Data.API;
using Client.ObjectModels.Logic.API;

namespace Client.Logic.Tests
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
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Laptop", 150, 8);
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
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Tablet", 120, 6);
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
            DummyProductDataTransferObject item = new DummyProductDataTransferObject(Guid.NewGuid(), "Smartwatch", 50, 2);
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
    }
}