using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace LogicTests
{
    [TestClass]
    public class DummyShopServiceTests
    {
        private StubDataRepository _stubRepository;
        private DummyShopService _shopService;

        [TestInitialize]
        public void Initialize()
        {
            _stubRepository = new StubDataRepository();
            _shopService = new DummyShopService(_stubRepository);
        }

        [TestMethod]
        public void GetAvailableProducts_ReturnsAllProductsFromRepository()
        {
            // Arrange
            int expectedProductCount = _stubRepository.GetAllProducts().Count;

            // Act
            List<IProduct> products = _shopService.GetAvailableProducts();

            // Assert
            Assert.AreEqual(expectedProductCount, products.Count);
            CollectionAssert.AreEqual(_stubRepository.GetAllProducts(), products);
        }

        [TestMethod]
        public void PurchaseProduct_DecreasesStock_WhenProductAvailable()
        {
            // Arrange
            string productName = "Produkt testowy 1";
            int initialStock = _shopService.GetStock(productName);

            // Act
            bool result = _shopService.PurchaseProduct(productName);
            int newStock = _shopService.GetStock(productName);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(initialStock - 1, newStock);
        }

        [TestMethod]
        public void PurchaseProduct_ReturnsFalse_WhenProductNotInStock()
        {
            // Arrange
            string nonExistentProduct = "Nieistniejący produkt";

            // Act
            bool result = _shopService.PurchaseProduct(nonExistentProduct);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetStock_ReturnsCorrectValue_ForExistingProduct()
        {
            // Arrange
            string productName = "Produkt testowy 1";

            // Act
            int stock = _shopService.GetStock(productName);

            // Assert
            Assert.AreEqual(10, stock); // Początkowy stan magazynu to 10
        }

        [TestMethod]
        public void GetStock_ReturnsZero_ForNonExistingProduct()
        {
            // Arrange
            string nonExistentProduct = "Nieistniejący produkt";

            // Act
            int stock = _shopService.GetStock(nonExistentProduct);

            // Assert
            Assert.AreEqual(0, stock);
        }
    }
}
