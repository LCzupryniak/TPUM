using Client.Presentation.Model.API;

namespace Client.Presentation.ViewModel.Tests
{
    [TestClass]
    public sealed class MainViewModelTests
    {
        private ICartModelService _inventoryModelService = null!;
        private ICustomerModelService _customerModelService = null!;
        private IProductModelService _itemModelService = null!;
        private IOrderModelService _orderModelService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _inventoryModelService = new DummyInventoryModelService();
            _customerModelService = new DummyCustomerModelService(_inventoryModelService);
            _itemModelService = new DummyItemModelService();
            _orderModelService = new DummyOrderModelService(_customerModelService, _itemModelService);
        }

        [TestMethod]
        public void Create_TestMethod()
        {
            MainViewModel vm = new MainViewModel(_customerModelService, _itemModelService, _orderModelService);

            Assert.IsNotNull(vm);
            Assert.IsFalse(vm.Customers.Any());
            Assert.IsFalse(vm.ShopItems.Any());
            Assert.IsFalse(vm.SelectedCustomerInventory.Any());
            Assert.IsFalse(vm.Orders.Any());

            Assert.IsNull(vm.SelectedCustomer);
            Assert.IsNull(vm.SelectedShopItem);

            Assert.IsFalse(vm.BuyItemCommand.CanExecute(null));
        }

        [TestMethod]
        public void Action_TestMethod()
        {
            MainViewModel vm = new MainViewModel(_customerModelService, _itemModelService, _orderModelService);

            Assert.IsFalse(vm.BuyItemCommand.CanExecute(null));

            Guid invGuid = Guid.NewGuid();

            _inventoryModelService.Add(invGuid, 25);

            Guid customerGuid = Guid.NewGuid();

            _customerModelService.AddCustomer(customerGuid, "Test", 200, invGuid);

            ICustomerModel? customer = _customerModelService.GetCustomer(customerGuid);
            Assert.IsNotNull(customer);

            Guid itemGuid = Guid.NewGuid();
            _itemModelService.AddItem(itemGuid, "Test", 15, 10);

            IProductModel? item = _itemModelService.GetItem(itemGuid);

            Assert.IsFalse(vm.BuyItemCommand.CanExecute(null));

            vm.SelectedCustomer = customer;
            vm.SelectedShopItem = item;

            Assert.IsTrue(vm.BuyItemCommand.CanExecute(null));
        }
    }
}
