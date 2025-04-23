using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;
using Client.Presentation.Model.Tests;

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
            _itemDto1 = new DummyItemDto { Id = _item1Id, Name = "Smartphone", Price = 50, MaintenanceCost = 2 };
            _itemDto2 = new DummyItemDto { Id = _item2Id, Name = "Smartwatch", Price = 30, MaintenanceCost = 1 };
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
            Assert.AreEqual("Smartphone", item1Model.Name);
            Assert.AreEqual(50, item1Model.Price);
            Assert.AreEqual(2, item1Model.MaintenanceCost);
        }

        [TestMethod]
        public void GetItem_ExistingId_ReturnsCorrectMappedItemModel()
        {
            IProductModel? item = _itemModelService.GetItem(_item1Id);

            Assert.IsNotNull(item);
            Assert.AreEqual(_item1Id, item.Id);
            Assert.AreEqual("Smartphone", item.Name);
            Assert.AreEqual(50, item.Price);
            Assert.AreEqual(2, item.MaintenanceCost);
        }
    }
}