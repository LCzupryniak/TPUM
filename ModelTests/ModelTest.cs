using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.ComponentModel;


namespace ModelTests
    {
        [TestClass]
        public class ProductViewModelTests
        {
            private ProductViewModel _productViewModel;

            [TestInitialize]
            public void Initialize()
            {
                _productViewModel = new ProductViewModel
                {
                    Name = "Testowy Produkt",
                    Price = 199.99,
                    Stock = 10,
                    SelectedQuantity = 1
                };
            }

            [TestMethod]
            public void Constructor_InitializesObject()
            {
                // Arrange & Act
                var viewModel = new ProductViewModel();

                // Assert
                Assert.IsNotNull(viewModel);
                Assert.IsNull(viewModel.Name);
                Assert.AreEqual(0, viewModel.Price);
                Assert.AreEqual(0, viewModel.Stock);
                Assert.AreEqual(0, viewModel.SelectedQuantity);
            }

            [TestMethod]
            public void Name_SetAndGet_WorksCorrectly()
            {
                // Arrange
                string expectedName = "Nowy Produkt";

                // Act
                _productViewModel.Name = expectedName;

                // Assert
                Assert.AreEqual(expectedName, _productViewModel.Name);
            }

            [TestMethod]
            public void Price_SetAndGet_WorksCorrectly()
            {
                // Arrange
                double expectedPrice = 299.99;

                // Act
                _productViewModel.Price = expectedPrice;

                // Assert
                Assert.AreEqual(expectedPrice, _productViewModel.Price);
            }

            [TestMethod]
            public void Stock_SetAndGet_WorksCorrectly()
            {
                // Arrange
                int expectedStock = 20;

                // Act
                _productViewModel.Stock = expectedStock;

                // Assert
                Assert.AreEqual(expectedStock, _productViewModel.Stock);
            }

            [TestMethod]
            public void SelectedQuantity_SetAndGet_WorksCorrectly()
            {
                // Arrange
                int expectedQuantity = 5;

                // Act
                _productViewModel.SelectedQuantity = expectedQuantity;

                // Assert
                Assert.AreEqual(expectedQuantity, _productViewModel.SelectedQuantity);
            }

            [TestMethod]
            public void Stock_RaisesPropertyChangedEvent_WhenValueChanges()
            {
                // Arrange
                bool propertyChangedRaised = false;
                string propertyNameRaised = null;

                _productViewModel.PropertyChanged += new PropertyChangedEventHandler(
                    delegate (object sender, PropertyChangedEventArgs e)
                    {
                        propertyChangedRaised = true;
                        propertyNameRaised = e.PropertyName;
                    }
                );

                // Act
                _productViewModel.Stock = 25;

                // Assert
                Assert.IsTrue(propertyChangedRaised);
                Assert.AreEqual("Stock", propertyNameRaised);
            }

            [TestMethod]
            public void SelectedQuantity_RaisesPropertyChangedEvent_WhenValueChanges()
            {
                // Arrange
                bool propertyChangedRaised = false;
                string propertyNameRaised = null;

                _productViewModel.PropertyChanged += new PropertyChangedEventHandler(
                    delegate (object sender, PropertyChangedEventArgs e)
                    {
                        propertyChangedRaised = true;
                        propertyNameRaised = e.PropertyName;
                    }
                );

                // Act
                _productViewModel.SelectedQuantity = 3;

                // Assert
                Assert.IsTrue(propertyChangedRaised);
                Assert.AreEqual("SelectedQuantity", propertyNameRaised);
            }

            [TestMethod]
            public void Name_DoesNotRaisePropertyChangedEvent_WhenValueChanges()
            {
                // Arrange
                bool propertyChangedRaised = false;

                _productViewModel.PropertyChanged += new PropertyChangedEventHandler(
                    delegate (object sender, PropertyChangedEventArgs e)
                    {
                        propertyChangedRaised = true;
                    }
                );

                // Act
                _productViewModel.Name = "Inny Nazwa Produktu";

                // Assert
                Assert.IsFalse(propertyChangedRaised);
            }

            [TestMethod]
            public void Price_DoesNotRaisePropertyChangedEvent_WhenValueChanges()
            {
                // Arrange
                bool propertyChangedRaised = false;

                _productViewModel.PropertyChanged += new PropertyChangedEventHandler(
                    delegate (object sender, PropertyChangedEventArgs e)
                    {
                        propertyChangedRaised = true;
                    }
                );

                // Act
                _productViewModel.Price = 399.99;

                // Assert
                Assert.IsFalse(propertyChangedRaised);
            }

        }
    }
