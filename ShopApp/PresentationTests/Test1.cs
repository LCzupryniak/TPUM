using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PresentationTests.Tests
{
    // Klasa bazowa do testów ViewModeli
    public abstract class TestBase
    {
        protected IOrderService orderService;
        protected IProductService productService;
        protected IUserService userService;
        protected IShoppingCartService cartService;

        protected ApplicationController controller;

        [TestInitialize]
        public virtual void Setup()
        {
            // Inicjalizacja interfejsów z implementacjami testowymi
            orderService = new TestOrderService();
            productService = new TestProductService();
            userService = new TestUserService();
            cartService = new TestShoppingCartService(productService, orderService);

            // Utworzenie kontrolera aplikacji z usługami testowymi
            controller = new ApplicationController(orderService, productService, userService, cartService);
        }
    }

    /// <summary>
    /// Klasy testowe usług
    /// </summary>

    // Testowa implementacja IOrderService
    public class TestOrderService : IOrderService
    {
        private List<OrderModel> _orders;

        public TestOrderService()
        {
            _orders = new List<OrderModel>
            {
                new OrderModel
                {
                    Id = 1,
                    CustomerName = "Jan Testowy",
                    Description = "Zamówienie testowe 1",
                    OrderDate = DateTime.Now.AddDays(-5),
                    TotalAmount = 150.75m,
                    Status = "Nowe",
                    Items = new List<OrderItemModel>
                    {
                        new OrderItemModel
                        {
                            Id = 1,
                            ProductName = "Produkt testowy 1",
                            ProductDescription = "Opis testowy 1",
                            UnitPrice = 50.25m,
                            Quantity = 3,
                            TotalPrice = 150.75m
                        }
                    }
                },
                new OrderModel
                {
                    Id = 2,
                    CustomerName = "Anna Testowa",
                    Description = "Zamówienie testowe 2",
                    OrderDate = DateTime.Now.AddDays(-2),
                    TotalAmount = 75.50m,
                    Status = "W realizacji",
                    Items = new List<OrderItemModel>
                    {
                        new OrderItemModel
                        {
                            Id = 2,
                            ProductName = "Produkt testowy 2",
                            ProductDescription = "Opis testowy 2",
                            UnitPrice = 75.50m,
                            Quantity = 1,
                            TotalPrice = 75.50m
                        }
                    }
                }
            };
        }

        public List<OrderModel> GetOrders()
        {
            return _orders;
        }

        public OrderModel GetOrderById(int orderId)
        {
            return _orders.FirstOrDefault(o => o.Id == orderId);
        }

        public bool ProcessOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.Status = "W realizacji";
                return true;
            }
            return false;
        }

        public bool CancelOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.Status = "Anulowane";
                return true;
            }
            return false;
        }

        public int CreateOrder(OrderModel order)
        {
            int newId = _orders.Max(o => o.Id) + 1;
            order.Id = newId;
            _orders.Add(order);
            return newId;
        }
    }

    // Testowa implementacja IProductService
    public class TestProductService : IProductService
    {
        private List<ProductModel> _products;
        private List<string> _categories;

        public TestProductService()
        {
            _categories = new List<string> { "Elektronika", "Odzież", "Książki" };

            _products = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = 1,
                    Name = "Laptop testowy",
                    Description = "Laptop do testów",
                    Category = "Elektronika",
                    Price = 2500.00m,
                    StockQuantity = 10
                },
                new ProductModel
                {
                    Id = 2,
                    Name = "Koszulka testowa",
                    Description = "Koszulka do testów",
                    Category = "Odzież",
                    Price = 50.00m,
                    StockQuantity = 100
                },
                new ProductModel
                {
                    Id = 3,
                    Name = "Książka testowa",
                    Description = "Książka do testów",
                    Category = "Książki",
                    Price = 35.99m,
                    StockQuantity = 50
                }
            };
        }

        public List<ProductModel> GetProducts()
        {
            return _products;
        }

        public List<ProductModel> GetProductsByCategory(string category)
        {
            return _products.Where(p => p.Category == category).ToList();
        }

        public List<ProductModel> SearchProducts(string query, string category)
        {
            var results = _products.Where(p =>
                (string.IsNullOrEmpty(query) ||
                p.Name.Contains(query) ||
                p.Description.Contains(query)) &&
                (string.IsNullOrEmpty(category) ||
                p.Category == category)
            ).ToList();

            return results;
        }

        public ProductModel GetProductById(int productId)
        {
            return _products.FirstOrDefault(p => p.Id == productId);
        }

        public List<string> GetCategories()
        {
            return _categories;
        }
    }

    // Testowa implementacja IUserService
    public class TestUserService : IUserService
    {
        private UserModel _user;

        public TestUserService()
        {
            _user = new UserModel
            {
                Id = 1,
                Name = "Jan Testowy",
                Email = "jan.testowy@example.com",
                Address = "ul. Testowa 123, 00-001 Testowo",
                PhoneNumber = "+48 123 456 789",
                IsActive = true
            };
        }

        public UserModel GetCurrentUser()
        {
            return _user;
        }

        public bool UpdateUser(UserModel user)
        {
            if (user.Id == _user.Id)
            {
                _user = user;
                return true;
            }
            return false;
        }
    }

    // Testowa implementacja IShoppingCartService
    public class TestShoppingCartService : IShoppingCartService
    {
        private List<CartItemModel> _cartItems;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public TestShoppingCartService(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;

            _cartItems = new List<CartItemModel>();
        }

        public List<CartItemModel> GetCartItems()
        {
            return _cartItems;
        }

        public bool AddToCart(int productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.StockQuantity < quantity)
            {
                return false;
            }

            var existingItem = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.UnitPrice * existingItem.Quantity;
            }
            else
            {
                var newItem = new CartItemModel
                {
                    Id = _cartItems.Count > 0 ? _cartItems.Max(i => i.Id) + 1 : 1,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    TotalPrice = product.Price * quantity
                };
                _cartItems.Add(newItem);
            }

            return true;
        }

        public bool UpdateItemQuantity(int itemId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return false;
            }

            var product = _productService.GetProductById(item.ProductId);
            if (product == null || product.StockQuantity < quantity)
            {
                return false;
            }

            item.Quantity = quantity;
            item.TotalPrice = item.UnitPrice * quantity;

            return true;
        }

        public bool RemoveItem(int itemId)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return false;
            }

            _cartItems.Remove(item);
            return true;
        }

        public decimal GetCartTotal()
        {
            return _cartItems.Sum(i => i.TotalPrice);
        }

        public int Checkout()
        {
            if (_cartItems.Count == 0)
            {
                return -1;
            }

            var order = new OrderModel
            {
                CustomerName = "Jan Testowy", // W rzeczywistej aplikacji pobieralibyśmy to z usługi użytkownika
                Description = "Zamówienie testowe",
                OrderDate = DateTime.Now,
                TotalAmount = GetCartTotal(),
                Status = "Nowe",
                Items = _cartItems.Select(i => new OrderItemModel
                {
                    ProductName = i.ProductName,
                    ProductDescription = i.ProductDescription,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };

            int orderId = _orderService.CreateOrder(order);
            if (orderId > 0)
            {
                _cartItems.Clear();
            }

            return orderId;
        }
    }

    /// <summary>
    /// Modele danych dla testów
    /// </summary>

    // Model zamówienia
    public class OrderModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
    }

    // Model elementu zamówienia
    public class OrderItemModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    // Model produktu
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    // Model użytkownika
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }

    // Model elementu koszyka
    public class CartItemModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// Interfejsy usług
    /// </summary>

    // Interfejs IOrderService
    public interface IOrderService
    {
        List<OrderModel> GetOrders();
        OrderModel GetOrderById(int orderId);
        bool ProcessOrder(int orderId);
        bool CancelOrder(int orderId);
        int CreateOrder(OrderModel order);
    }

    // Interfejs IProductService
    public interface IProductService
    {
        List<ProductModel> GetProducts();
        List<ProductModel> GetProductsByCategory(string category);
        List<ProductModel> SearchProducts(string query, string category);
        ProductModel GetProductById(int productId);
        List<string> GetCategories();
    }

    // Interfejs IUserService
    public interface IUserService
    {
        UserModel GetCurrentUser();
        bool UpdateUser(UserModel user);
    }

    // Interfejs IShoppingCartService
    public interface IShoppingCartService
    {
        List<CartItemModel> GetCartItems();
        bool AddToCart(int productId, int quantity);
        bool UpdateItemQuantity(int itemId, int quantity);
        bool RemoveItem(int itemId);
        decimal GetCartTotal();
        int Checkout();
    }

    /// <summary>
    /// Szkielet ViewModeli do testów
    /// </summary>

    // Bazowa klasa ViewModelu
    public abstract class ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Symulacja interfejsu PropertyChangedEventHandler
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    // Symulacja PropertyChangedEventArgs
    public class PropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; private set; }

        public PropertyChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    /// <summary>
    /// Testy jednostkowe dla ViewModeli
    /// </summary>

    [TestClass]
    public class OrderViewModelTests : TestBase
    {
        private OrderViewModelWithCommands viewModel;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            viewModel = controller.CreateOrderViewModel();
        }

        [TestMethod]
        public void TestProcessOrderCommand_ChangesOrderStatus()
        {
            // Arrange
            var firstOrder = orderService.GetOrderById(1);
            viewModel.SelectedOrder = firstOrder;
            string initialStatus = firstOrder.Status;

            // Act
            viewModel.ProcessOrderCommand.Execute(1);

            // Assert
            Assert.AreNotEqual(initialStatus, viewModel.SelectedOrder.Status);
            Assert.AreEqual("W realizacji", viewModel.SelectedOrder.Status);
        }

        [TestMethod]
        public void TestCancelOrderCommand_ChangesOrderStatus()
        {
            // Arrange
            var firstOrder = orderService.GetOrderById(1);
            viewModel.SelectedOrder = firstOrder;
            string initialStatus = firstOrder.Status;

            // Act
            viewModel.CancelOrderCommand.Execute(1);

            // Assert
            Assert.AreNotEqual(initialStatus, viewModel.SelectedOrder.Status);
            Assert.AreEqual("Anulowane", viewModel.SelectedOrder.Status);
        }

        [TestMethod]
        public void TestCanExecuteCommands_WhenNoSelectedOrder_ReturnsFalse()
        {
            // Arrange
            viewModel.SelectedOrder = null;

            // Act & Assert
            Assert.IsFalse(viewModel.ProcessOrderCommand.CanExecute(1));
            Assert.IsFalse(viewModel.CancelOrderCommand.CanExecute(1));
        }
    }

    [TestClass]
    public class ProductCatalogViewModelTests : TestBase
    {
        private ProductCatalogViewModelWithCommands viewModel;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            viewModel = controller.CreateProductCatalogViewModel();
        }

        [TestMethod]
        public void TestSearchCommand_FiltersProducts()
        {
            // Arrange
            viewModel.SearchQuery = "Laptop";
            viewModel.SelectedCategory = null;

            // Act
            viewModel.SearchCommand.Execute(null);

            // Assert
            Assert.AreEqual(1, viewModel.Products.Count);
            Assert.AreEqual("Laptop testowy", viewModel.Products[0].Name);
        }

        [TestMethod]
        public void TestAddToCartCommand_AddsProductToCart()
        {
            // Arrange
            viewModel.SelectedProduct = productService.GetProductById(1);
            viewModel.Quantity = 2;
            var cartViewModel = controller.CreateShoppingCartViewModel();
            int initialCartItemsCount = cartViewModel.CartItems.Count;

            // Act
            viewModel.AddToCartCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCartItemsCount + 1, cartViewModel.CartItems.Count);
            Assert.AreEqual(viewModel.SelectedProduct.Name, cartViewModel.CartItems[0].ProductName);
            Assert.AreEqual(viewModel.Quantity, cartViewModel.CartItems[0].Quantity);
        }

        [TestMethod]
        public void TestCanExecuteCommands_WhenNoSelectedProduct_ReturnsFalse()
        {
            // Arrange
            viewModel.SelectedProduct = null;

            // Act & Assert
            Assert.IsFalse(viewModel.AddToCartCommand.CanExecute(null));
            Assert.IsFalse(viewModel.ShowProductDetailsCommand.CanExecute(null));
        }
    }

    [TestClass]
    public class ShoppingCartViewModelTests : TestBase
    {
        private ShoppingCartViewModelWithCommands viewModel;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            viewModel = controller.CreateShoppingCartViewModel();

            // Dodaj produkt do koszyka, aby mieć dane do testów
            cartService.AddToCart(1, 2);
        }

        [TestMethod]
        public void TestIncreaseQuantityCommand_IncreasesItemQuantity()
        {
            // Arrange
            var item = viewModel.CartItems[0];
            int initialQuantity = item.Quantity;

            // Act
            viewModel.IncreaseQuantityCommand.Execute(item.Id);

            // Assert
            Assert.AreEqual(initialQuantity + 1, viewModel.CartItems[0].Quantity);
        }

        [TestMethod]
        public void TestDecreaseQuantityCommand_DecreasesItemQuantity()
        {
            // Arrange
            var item = viewModel.CartItems[0];
            item.Quantity = 3; // Ustaw ilość na 3, aby zmniejszenie było możliwe
            int initialQuantity = item.Quantity;

            // Act
            viewModel.DecreaseQuantityCommand.Execute(item.Id);

            // Assert
            Assert.AreEqual(initialQuantity - 1, viewModel.CartItems[0].Quantity);
        }

        [TestMethod]
        public void TestRemoveItemCommand_RemovesItemFromCart()
        {
            // Arrange
            var item = viewModel.CartItems[0];
            int initialItemsCount = viewModel.CartItems.Count;

            // Act
            viewModel.RemoveItemCommand.Execute(item.Id);

            // Assert
            Assert.AreEqual(initialItemsCount - 1, viewModel.CartItems.Count);
        }

        [TestMethod]
        public void TestClearCartCommand_RemovesAllItemsFromCart()
        {
            // Arrange - dodaj więcej produktów do koszyka
            cartService.AddToCart(2, 1);
            cartService.AddToCart(3, 1);

            // Act
            viewModel.ClearCartCommand.Execute(null);

            // Assert
            Assert.AreEqual(0, viewModel.CartItems.Count);
            Assert.AreEqual(0, viewModel.TotalAmount);
        }

        [TestMethod]
        public void TestCheckoutCommand_CreatesOrderAndClearsCart()
        {
            // Arrange
            int initialOrdersCount = orderService.GetOrders().Count;

            // Act
            viewModel.CheckoutCommand.Execute(null);

            // Assert
            Assert.AreEqual(0, viewModel.CartItems.Count);
            Assert.AreEqual(initialOrdersCount + 1, orderService.GetOrders().Count);
        }

        [TestMethod]
        public void TestCanExecuteCheckoutCommand_WhenCartEmpty_ReturnsFalse()
        {
            // Arrange
            viewModel.ClearCartCommand.Execute(null);

            // Act & Assert
            Assert.IsFalse(viewModel.CheckoutCommand.CanExecute(null));
        }
    }

    [TestClass]
    public class UserViewModelTests : TestBase
    {
        private UserViewModelWithCommands viewModel;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            viewModel = controller.CreateUserViewModel();
        }

        [TestMethod]
        public void TestSaveCommand_UpdatesUserData()
        {
            // Arrange
            var originalName = viewModel.User.Name;
            viewModel.User.Name = "Nowe Imię Testowe";

            // Act
            viewModel.SaveCommand.Execute(null);

            // Assert
            var updatedUser = userService.GetCurrentUser();
            Assert.AreNotEqual(originalName, updatedUser.Name);
            Assert.AreEqual("Nowe Imię Testowe", updatedUser.Name);
        }

        [TestMethod]
        public void TestCancelCommand_ResetsUserData()
        {
            // Arrange
            var originalName = viewModel.User.Name;
            viewModel.User.Name = "Zmiana Która Powinna Być Cofnięta";

            // Act
            viewModel.CancelCommand.Execute(null);

            // Assert
            Assert.AreEqual(originalName, viewModel.User.Name);
        }
    }
}
