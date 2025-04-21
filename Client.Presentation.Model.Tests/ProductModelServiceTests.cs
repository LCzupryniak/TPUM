using Client.Presentation.Model.API;
using Client.Presentation.Model.Tests;
using ClientServer.Shared.Logic.API;

namespace Presentation.Model.Tests
{
    [TestClass]
    public class ProductModelServiceTests
    {
        private DummyItemLogic _dummyItemLogic = null!;
        private IProductModelService _itemModelService = null!;

        private Guid _item1Id, _item2Id;

        private DummyItemDto _itemDto1 = null!;
        private DummyItemDto _itemDto2 = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _dummyItemLogic = new DummyItemLogic();

            _item1Id = Guid.NewGuid();
            _item2Id = Guid.NewGuid();
            _itemDto1 = new DummyItemDto { Id = _item1Id, Name = "Smartwatch", Price = 50, MaintenanceCost = 2 };
            _itemDto2 = new DummyItemDto { Id = _item2Id, Name = "Laptop", Price = 30, MaintenanceCost = 1 };
            _dummyItemLogic.Items.Add(_itemDto1.Id, _itemDto1);
            _dummyItemLogic.Items.Add(_itemDto2.Id, _itemDto2);

            _itemModelService = ModelFactory.CreateItemModelService(_dummyItemLogic);
        }

        [TestMethod]
        public void GetAllItems_WhenCalled_ReturnsAllMappedItemModels()
        {
            List<IProductModel> items = _itemModelService.GetAllItems().ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count);

            // Verify mapping for one item
            IProductModel? item1Model = items.FirstOrDefault(i => i.Id == _item1Id);
            Assert.IsNotNull(item1Model);
            Assert.AreEqual(_item1Id, item1Model.Id);
            Assert.AreEqual("Smartwatch", item1Model.Name);
            Assert.AreEqual(50, item1Model.Price);
            Assert.AreEqual(2, item1Model.MaintenanceCost);
        }

        [TestMethod]
        public void GetItem_NonExistingId_ReturnsNull()
        {
            Guid nonExistingId = Guid.NewGuid();

            IProductModel? item = _itemModelService.GetItem(nonExistingId);

            Assert.IsNull(item);
        }

        [TestMethod]
        public void RemoveItem_ExistingId_CallsLogicRemoveAndReturnsTrue()
        {
            Guid targetId = _item1Id;
            Assert.IsTrue(_dummyItemLogic.Items.ContainsKey(targetId));

            bool result = _itemModelService.RemoveItem(targetId);

            Assert.IsTrue(result);
            Assert.IsFalse(_dummyItemLogic.Items.ContainsKey(targetId));
        }

        [TestMethod]
        public void RemoveItem_NonExistingId_CallsLogicRemoveAndReturnsFalse()
        {
            Guid nonExistingId = Guid.NewGuid();
            int initialCount = _dummyItemLogic.Items.Count;

            bool result = _itemModelService.RemoveItem(nonExistingId);

            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, _dummyItemLogic.Items.Count);
        }

        [TestMethod]
        public void UpdateItem_ExistingId_CallsLogicUpdateAndReturnsTrue()
        {
            Guid targetId = _item1Id;
            string updatedName = "Premium TV";
            int updatedPrice = 100;
            int updatedCost = 4;

            bool result = _itemModelService.UpdateItem(targetId, updatedName, updatedPrice, updatedCost);

            Assert.IsTrue(result);
            Assert.IsTrue(_dummyItemLogic.Items.ContainsKey(targetId));
            IProductDataTransferObject updatedDto = _dummyItemLogic.Items[targetId];
            Assert.AreEqual(updatedName, updatedDto.Name);
            Assert.AreEqual(updatedPrice, updatedDto.Price);
            Assert.AreEqual(updatedCost, updatedDto.MaintenanceCost);
        }

        [TestMethod]
        public void UpdateItem_NonExistingId_CallsLogicUpdateAndReturnsFalse()
        {
            Guid nonExistingId = Guid.NewGuid();
            int initialCount = _dummyItemLogic.Items.Count;

            bool result = _itemModelService.UpdateItem(nonExistingId, "Fake Product", 1, 0);

            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, _dummyItemLogic.Items.Count);
        }
    }
}