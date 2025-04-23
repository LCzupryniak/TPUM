using Server.ObjectModels.Data.API;
using Server.ObjectModels.Logic.API;
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
    }
}