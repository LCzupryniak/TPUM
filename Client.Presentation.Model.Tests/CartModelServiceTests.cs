using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Tests
{
    [TestClass]
    public class CartModelServiceTests
    {
        private DummyCartLogic _dummyCartLogic = null!;
        private DummyItemLogic _dummyItemLogic = null!;

        private ICartModelService _cartModelService = null!;

        private Guid _inv1Id, _inv2Id;
        private Guid _item1Id, _item2Id;

        private DummyCartDto _invDto1 = null!;
        private DummyCartDto _invDto2 = null!;
        private DummyItemDto _itemDto1 = null!;
        private DummyItemDto _itemDto2 = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            // Instantiate Dummies
            _dummyCartLogic = new DummyCartLogic();
            _dummyItemLogic = new DummyItemLogic();

            // Create Test DTOs
            _item1Id = Guid.NewGuid();
            _item2Id = Guid.NewGuid();
            _itemDto1 = new DummyItemDto { Id = _item1Id, Name = "Laptop", Price = 10, MaintenanceCost = 0 };
            _itemDto2 = new DummyItemDto { Id = _item2Id, Name = "Microphone", Price = 25, MaintenanceCost = 1 };
            _dummyItemLogic.Items.Add(_itemDto1.Id, _itemDto1);
            _dummyItemLogic.Items.Add(_itemDto2.Id, _itemDto2);

            _inv1Id = Guid.NewGuid();
            _inv2Id = Guid.NewGuid();
            _invDto1 = new DummyCartDto { Id = _inv1Id, Capacity = 10, Items = new List<IProductDataTransferObject> { _itemDto1, _itemDto2 } };
            _invDto2 = new DummyCartDto { Id = _inv2Id, Capacity = 5, Items = new List<IProductDataTransferObject>() }; // Empty
            _dummyCartLogic.Carts.Add(_invDto1.Id, _invDto1);
            _dummyCartLogic.Carts.Add(_invDto2.Id, _invDto2);

            _cartModelService = ModelFactory.CreateCartModelService(_dummyCartLogic);
        }

        [TestMethod]
        public void GetAllCarts_WhenCalled_ReturnsAllMappedCartModels()
        {
            List<ICartModel> carts = _cartModelService.GetAllCarts().ToList();

            Assert.IsNotNull(carts);
            Assert.AreEqual(2, carts.Count);

            // Verify mapping for one cart
            ICartModel? inv1Model = carts.FirstOrDefault(i => i.Id == _inv1Id);
            Assert.IsNotNull(inv1Model);
            Assert.AreEqual(_inv1Id, inv1Model.Id);
            Assert.AreEqual(10, inv1Model.Capacity);
            Assert.IsNotNull(inv1Model.Items);
            Assert.AreEqual(2, inv1Model.Items.Count());

            IProductModel? item1Model = inv1Model.Items.FirstOrDefault(itm => itm.Id == _item1Id);
            Assert.IsNotNull(item1Model);
            Assert.AreEqual("Laptop", item1Model.Name);
            Assert.AreEqual(10, item1Model.Price);
        }

        [TestMethod]
        public void GetCart_ExistingId_ReturnsCorrectMappedCartModel()
        {
            ICartModel? cart = _cartModelService.GetCart(_inv1Id);

            Assert.IsNotNull(cart);
            Assert.AreEqual(_inv1Id, cart.Id);
            Assert.AreEqual(10, cart.Capacity);
            Assert.IsNotNull(cart.Items);
            Assert.AreEqual(2, cart.Items.Count());
            Assert.AreEqual("Laptop", cart.Items.First().Name);
        }

        [TestMethod]
        public void GetCart_ExistingEmptyCart_ReturnsMappedCartModelWithEmptyItems()
        {
            ICartModel? cart = _cartModelService.GetCart(_inv2Id);

            Assert.IsNotNull(cart);
            Assert.AreEqual(_inv2Id, cart.Id);
            Assert.AreEqual(5, cart.Capacity);
            Assert.IsNotNull(cart.Items);
            Assert.AreEqual(0, cart.Items.Count());
        }

        [TestMethod]
        public void GetCart_NonExistingId_ReturnsNull()
        {
            Guid nonExistingId = Guid.NewGuid();

            ICartModel? cart = _cartModelService.GetCart(nonExistingId);

            Assert.IsNull(cart);
        }
    }
}