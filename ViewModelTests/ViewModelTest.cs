using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;
using ViewModelTests;
using System.Linq;

namespace ViewModelTests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void LoadProducts_ShouldInitializeCorrectly()
        {
            var shopService = new DummyShopModelService();
            shopService.ProductsToReturn.Add(new DummyProductModel { Name = "ProductA", Price = 9.99m });

            var notifier = new DummyProductStockModelNotifier();
            notifier.Stock["ProductA"] = 5;

            var viewModel = new MainViewModel(shopService, notifier);

            Assert.AreEqual(1, viewModel.Products.Count);
            Assert.AreEqual("ProductA", viewModel.Products[0].Name);
            Assert.AreEqual(5, viewModel.Products[0].Stock);
        }

        [TestMethod]
        public void PurchaseCommand_ShouldBuySelectedProducts()
        {
            var shopService = new DummyShopModelService();
            shopService.ProductsToReturn.Add(new DummyProductModel { Name = "ProductA", Price = 9.99m });

            var notifier = new DummyProductStockModelNotifier();
            notifier.Stock["ProductA"] = 10;

            var viewModel = new MainViewModel(shopService, notifier);
            viewModel.Products[0].SelectedQuantity = 2;

            viewModel.PurchaseCommand.Execute(null);

            Assert.AreEqual(2, shopService.PurchasedProducts.Count);
            Assert.AreEqual(8, viewModel.Products[0].Stock);
            Assert.AreEqual(0, viewModel.Products[0].SelectedQuantity);
        }

        [TestMethod]
        public void StockChangedEvent_ShouldUpdateStockInViewModel()
        {
            var shopService = new DummyShopModelService();
            shopService.ProductsToReturn.Add(new DummyProductModel { Name = "ProductA", Price = 9.99m });

            var notifier = new DummyProductStockModelNotifier();
            notifier.Stock["ProductA"] = 10;

            var viewModel = new MainViewModel(shopService, notifier);
            notifier.Stock["ProductA"] = 3;

            notifier.RaiseStockChanged();

            Assert.AreEqual(3, viewModel.Products[0].Stock);
        }
    }
}
