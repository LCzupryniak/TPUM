using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Interfaces;
using Logic.Interfaces;

namespace LogicTests.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private IOrderService _orderService;
        private IUserService _userService;
        private IProductService _productService;
        private IShoppingCartService _cartService;

        [TestInitialize]
        public void Setup()
        {
            // Użyj factory lub innego mechanizmu do tworzenia instancji serwisów
            _orderService = ServiceFactory.CreateOrderService();
            _userService = ServiceFactory.CreateUserService();
            _productService = ServiceFactory.CreateProductService();
            _cartService = ServiceFactory.CreateShoppingCartService(_productService, _orderService);

            // Rejestracja użytkownika testowego
            _userService.RegisterUser("Jan Kowalski", "jan@example.com", "ul. Testowa 1", "123456789");
        }

        [TestMethod]
        public void GetOrderById_OrderExists_ReturnsCorrectOrder()
        {
            // Arrange
            IUser user = _userService.GetUserByEmail("jan@example.com");
            IOrder newOrder = CreateTestOrder(user, "Zamówienie testowe");
            int orderId = newOrder.Id;

            // Act
            IOrder retrievedOrder = _orderService.GetOrderById(orderId);

            // Assert
            Assert.IsNotNull(retrievedOrder);
            Assert.AreEqual(orderId, retrievedOrder.Id);
            Assert.AreEqual("Zamówienie testowe", retrievedOrder.Description);
            Assert.AreEqual(user.Id, retrievedOrder.Customer.Id);
        }

        [TestMethod]
        public void GetOrderById_OrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            int nonExistentOrderId = 9999;

            // Act
            IOrder retrievedOrder = _orderService.GetOrderById(nonExistentOrderId);

            // Assert
            Assert.IsNull(retrievedOrder);
        }

        [TestMethod]
        public void GetOrdersByUser_UserHasOrders_ReturnsUserOrders()
        {
            // Arrange
            IUser user = _userService.GetUserByEmail("jan@example.com");
            CreateTestOrder(user, "Zamówienie 1");
            CreateTestOrder(user, "Zamówienie 2");

            // Act
            IEnumerable<IOrder> userOrders = _orderService.GetOrdersByUser(user);

            // Assert
            Assert.IsNotNull(userOrders);
            Assert.AreEqual(2, userOrders.Count());
        }

        [TestMethod]
        public void UpdateOrderStatus_OrderExists_StatusUpdated()
        {
            // Arrange
            IUser user = _userService.GetUserByEmail("jan@example.com");
            IOrder order = CreateTestOrder(user, "Zamówienie testowe");

            // Act
            _orderService.UpdateOrderStatus(order.Id, OrderStatus.Shipped);
            IOrder updatedOrder = _orderService.GetOrderById(order.Id);

            // Assert
            Assert.AreEqual(OrderStatus.Shipped, updatedOrder.Status);
        }

        [TestMethod]
        public void CancelOrder_OrderExists_StatusChangedToCancelled()
        {
            // Arrange
            IUser user = _userService.GetUserByEmail("jan@example.com");
            IOrder order = CreateTestOrder(user, "Zamówienie testowe");

            // Act
            _orderService.CancelOrder(order.Id);
            IOrder cancelledOrder = _orderService.GetOrderById(order.Id);

            // Assert
            Assert.AreEqual(OrderStatus.Cancelled, cancelledOrder.Status);
        }

        [TestMethod]
        public void ProcessOrder_ValidOrder_StatusChangedToProcessing()
        {
            // Arrange
            IUser user = _userService.GetUserByEmail("jan@example.com");
            IOrder order = CreateTestOrder(user, "Zamówienie testowe");

            // Act
            _orderService.ProcessOrder(order);
            IOrder processedOrder = _orderService.GetOrderById(order.Id);

            // Assert
            Assert.AreEqual(OrderStatus.Processing, processedOrder.Status);
        }

        private IOrder CreateTestOrder(IUser user, string description)
        {
            // Dodanie produktu do koszyka
            IProduct product = _productService.GetProductById(1);
            _cartService.AddToCart(user, product, 1);

            // Utworzenie zamówienia
            return _cartService.CreateOrderFromCart(user, description);
        }
    }

    [TestClass]
    public class ProductServiceTests
    {
        private IProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            _productService = ServiceFactory.CreateProductService();
        }

        [TestMethod]
        public void GetAllProducts_ReturnsAllProducts()
        {
            // Act
            IEnumerable<IProduct> products = _productService.GetAllProducts();

            // Assert
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count() > 0);
        }

        [TestMethod]
        public void GetProductById_ProductExists_ReturnsCorrectProduct()
        {
            // Arrange
            int productId = 1;

            // Act
            IProduct product = _productService.GetProductById(productId);

            // Assert
            Assert.IsNotNull(product);
            Assert.AreEqual(productId, product.Id);
            Assert.AreEqual("Telewizor LED 55\"", product.Name);
        }

        [TestMethod]
        public void GetProductById_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            int nonExistentProductId = 9999;

            // Act
            IProduct product = _productService.GetProductById(nonExistentProductId);

            // Assert
            Assert.IsNull(product);
        }

        [TestMethod]
        public void GetProductsByCategory_ReturnsProductsInCategory()
        {
            // Arrange
            ProductCategory category = ProductCategory.Electronics;

            // Act
            IEnumerable<IProduct> products = _productService.GetProductsByCategory(category);

            // Assert
            Assert.IsNotNull(products);
            foreach (IProduct product in products)
            {
                Assert.AreEqual(category, product.Category);
            }
        }

        [TestMethod]
        public void UpdateProductStock_ProductExists_StockIsUpdated()
        {
            // Arrange
            int productId = 1;
            int newQuantity = 25;

            // Act
            _productService.UpdateProductStock(productId, newQuantity);
            IProduct updatedProduct = _productService.GetProductById(productId);

            // Assert
            Assert.AreEqual(newQuantity, updatedProduct.StockQuantity);
        }
    }

    [TestClass]
    public class UserServiceTests
    {
        private IUserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _userService = ServiceFactory.CreateUserService();
        }

        [TestMethod]
        public void RegisterUser_NewUser_UserIsRegistered()
        {
            // Arrange
            string name = "Anna Nowak";
            string email = "anna@example.com";
            string address = "ul. Przykładowa 123";
            string phoneNumber = "987654321";

            // Act
            _userService.RegisterUser(name, email, address, phoneNumber);
            IUser registeredUser = _userService.GetUserByEmail(email);

            // Assert
            Assert.IsNotNull(registeredUser);
            Assert.AreEqual(name, registeredUser.Name);
            Assert.AreEqual(email, registeredUser.Email);
            Assert.AreEqual(address, registeredUser.Address);
            Assert.AreEqual(phoneNumber, registeredUser.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterUser_EmailAlreadyExists_ThrowsException()
        {
            // Arrange
            string name = "Jan Kowalski";
            string email = "jan@example.com";
            string address = "ul. Testowa 1";
            string phoneNumber = "123456789";

            // Act
            _userService.RegisterUser(name, email, address, phoneNumber);
            _userService.RegisterUser("Drugi Jan", email, "Inny adres", "111222333");

            // Assert - oczekiwanie na wyjątek
        }

        [TestMethod]
        public void GetUserById_UserExists_ReturnsUser()
        {
            // Arrange
            string name = "Jan Kowalski";
            string email = "jan@example.com";
            string address = "ul. Testowa 1";
            string phoneNumber = "123456789";
            _userService.RegisterUser(name, email, address, phoneNumber);
            IUser registeredUser = _userService.GetUserByEmail(email);
            int userId = registeredUser.Id;

            // Act
            IUser retrievedUser = _userService.GetUserById(userId);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(userId, retrievedUser.Id);
            Assert.AreEqual(name, retrievedUser.Name);
        }

        [TestMethod]
        public void GetUserById_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            int nonExistentUserId = 9999;

            // Act
            IUser user = _userService.GetUserById(nonExistentUserId);

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_UserExists_ReturnsUser()
        {
            // Arrange
            string name = "Jan Kowalski";
            string email = "jan@example.com";
            string address = "ul. Testowa 1";
            string phoneNumber = "123456789";
            _userService.RegisterUser(name, email, address, phoneNumber);

            // Act
            IUser user = _userService.GetUserByEmail(email);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(name, user.Name);
            Assert.AreEqual(email, user.Email);
        }

        [TestMethod]
        public void GetUserByEmail_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            string nonExistentEmail = "nieistniejacy@example.com";

            // Act
            IUser user = _userService.GetUserByEmail(nonExistentEmail);

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void UpdateUserStatus_UserExists_StatusUpdated()
        {
            // Arrange
            string name = "Jan Kowalski";
            string email = "jan@example.com";
            string address = "ul. Testowa 1";
            string phoneNumber = "123456789";
            _userService.RegisterUser(name, email, address, phoneNumber);
            IUser user = _userService.GetUserByEmail(email);

            // Act
            _userService.UpdateUserStatus(user.Id, false);
        }
    }

    [TestClass]
    public class ShoppingCartServiceTests
    {
        private IProductService _productService;
        private IOrderService _orderService;
        private IUserService _userService;
        private IShoppingCartService _cartService;
        private IUser _testUser;

        [TestInitialize]
        public void Setup()
        {
            _productService = ServiceFactory.CreateProductService();
            _orderService = ServiceFactory.CreateOrderService();
            _userService = ServiceFactory.CreateUserService();
            _cartService = ServiceFactory.CreateShoppingCartService(_productService, _orderService);

            // Rejestracja użytkownika testowego
            _userService.RegisterUser("Jan Kowalski", "jan@example.com", "ul. Testowa 1", "123456789");
            _testUser = _userService.GetUserByEmail("jan@example.com");
        }

        [TestMethod]
        public void AddToCart_NewProduct_ProductAddedToCart()
        {
            // Arrange
            IProduct product = _productService.GetProductById(1);
            int quantity = 2;

            // Act
            _cartService.AddToCart(_testUser, product, quantity);

        }

        [TestMethod]
        public void AddToCart_ExistingProduct_QuantityIncreased()
        {
            // Arrange
            IProduct product = _productService.GetProductById(1);

            // Act
            _cartService.AddToCart(_testUser, product, 1);
            _cartService.AddToCart(_testUser, product, 2);

        }

        [TestMethod]
        public void RemoveFromCart_ProductInCart_ProductRemoved()
        {
            // Arrange
            IProduct product = _productService.GetProductById(1);
            _cartService.AddToCart(_testUser, product, 1);

            // Act
            _cartService.RemoveFromCart(_testUser, product.Id);

        }

        [TestMethod]
        public void ClearCart_CartHasItems_CartCleared()
        {
            // Arrange
            IProduct product1 = _productService.GetProductById(1);
            IProduct product2 = _productService.GetProductById(2);
            _cartService.AddToCart(_testUser, product1, 1);
            _cartService.AddToCart(_testUser, product2, 2);

            // Act
            _cartService.ClearCart(_testUser);

        }

        [TestMethod]
        public void CreateOrderFromCart_CartHasItems_OrderCreated()
        {
            // Arrange
            IProduct product = _productService.GetProductById(1);
            _cartService.AddToCart(_testUser, product, 2);
            string orderDescription = "Zamówienie testowe";

            // Act
            IOrder order = _cartService.CreateOrderFromCart(_testUser, orderDescription);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(orderDescription, order.Description);
            Assert.AreEqual(_testUser.Id, order.Customer.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateOrderFromCart_EmptyCart_ThrowsException()
        {
            // Arrange
            _cartService.ClearCart(_testUser);
            string orderDescription = "Zamówienie testowe";

            // Act
            IOrder order = _cartService.CreateOrderFromCart(_testUser, orderDescription);

            // Assert - oczekiwanie na wyjątek
        }
    }

    // Klasa pomocnicza do tworzenia instancji serwisów
    public static class ServiceFactory
    {
        public static IOrderService CreateOrderService()
        {
            // Zastąp bezpośrednie tworzenie instancji odpowiednim mechanizmem tworzenia serwisów
            return DependencyInjector.GetService<IOrderService>();
        }

        public static IUserService CreateUserService()
        {
            return DependencyInjector.GetService<IUserService>();
        }

        public static IProductService CreateProductService()
        {
            return DependencyInjector.GetService<IProductService>();
        }

        public static IShoppingCartService CreateShoppingCartService(IProductService productService, IOrderService orderService)
        {
            return DependencyInjector.GetService<IShoppingCartService>(productService, orderService);
        }
    }

    // Symulacja kontenera DI
    public static class DependencyInjector
    {
        public static T GetService<T>()
        {
            if (typeof(T) == typeof(IOrderService))
            {
                return (T)(object)new OrderServiceForTest();
            }
            if (typeof(T) == typeof(IUserService))
            {
                return (T)(object)new UserServiceForTest();
            }
            if (typeof(T) == typeof(IProductService))
            {
                return (T)(object)new ProductServiceForTest();
            }

            throw new InvalidOperationException($"Nie można utworzyć instancji serwisu typu {typeof(T).Name}");
        }

        public static IShoppingCartService GetService<T>(IProductService productService, IOrderService orderService)
        {
            if (typeof(T) == typeof(IShoppingCartService))
            {
                return new ShoppingCartServiceForTest(productService, orderService);
            }

            throw new InvalidOperationException($"Nie można utworzyć instancji serwisu typu {typeof(T).Name}");
        }
    }

    internal class OrderServiceForTest : IOrderService
    {
        private Dictionary<int, OrderData> _orders = new Dictionary<int, OrderData>();
        private int _nextOrderId = 1;

        public void ProcessOrder(IOrder order)
        {
            if (_orders.TryGetValue(order.Id, out var orderData))
            {
                orderData.Status = OrderStatus.Processing;
            }
        }

        public IOrder GetOrderById(int id)
        {
            if (_orders.TryGetValue(id, out var orderData))
            {
                return new TestOrder(orderData);
            }
            return null;
        }

        public IEnumerable<IOrder> GetOrdersByUser(IUser user)
        {
            return _orders.Values
                .Where(o => o.CustomerId == user.Id)
                .Select(o => (IOrder)new TestOrder(o))
                .ToList();
        }

        public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            if (_orders.TryGetValue(orderId, out var orderData))
            {
                orderData.Status = newStatus;
            }
        }

        public void CancelOrder(int orderId)
        {
            if (_orders.TryGetValue(orderId, out var orderData))
            {
                orderData.Status = OrderStatus.Cancelled;
            }
        }

        public int CreateOrder(IUser user, string description)
        {
            var orderData = new OrderData
            {
                Id = _nextOrderId,
                Description = description,
                CustomerId = user.Id,
                Customer = user,
                Status = OrderStatus.New
            };

            _orders.Add(_nextOrderId, orderData);
            _nextOrderId++;
            return orderData.Id;
        }

        private class OrderData
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int CustomerId { get; set; }
            public IUser Customer { get; set; }
            public OrderStatus Status { get; set; }
            public List<OrderItemData> Items { get; set; } = new List<OrderItemData>();
        }

        private class OrderItemData
        {
            public int ProductId { get; set; }
            public IProduct Product { get; set; }
            public int Quantity { get; set; }
        }

        private class TestOrder : IOrder
        {
            private readonly OrderData _data;

            public TestOrder(OrderData data)
            {
                _data = data;
            }

            public int Id => _data.Id;
            public string Description => _data.Description;
            public IUser Customer => _data.Customer;
            public OrderStatus Status => _data.Status;
            public decimal TotalAmount => 0; 
            public DateTime OrderDate => DateTime.Now; 
        }
    }

    internal class ProductServiceForTest : IProductService
    {
        private Dictionary<int, ProductData> _products = new Dictionary<int, ProductData>();

        public ProductServiceForTest()
        {
            InitializeProductCatalog();
        }

        private void InitializeProductCatalog()
        {
            AddProduct(new ProductData
            {
                Id = 1,
                Name = "Telewizor LED 55\"",
                Description = "Telewizor o wysokiej rozdzielczości",
                Price = 2499.99m,
                StockQuantity = 10,
                Category = ProductCategory.Electronics
            });

            AddProduct(new ProductData
            {
                Id = 2,
                Name = "Koszulka bawełniana",
                Description = "Koszulka z czystej bawełny",
                Price = 49.99m,
                StockQuantity = 100,
                Category = ProductCategory.Clothing
            });

            AddProduct(new ProductData
            {
                Id = 3,
                Name = "Programowanie w C#",
                Description = "Książka dla początkujących programistów",
                Price = 89.99m,
                StockQuantity = 25,
                Category = ProductCategory.Books
            });
        }

        private void AddProduct(ProductData product)
        {
            _products.Add(product.Id, product);
        }

        public IEnumerable<IProduct> GetAllProducts()
        {
            return _products.Values
                .Select(p => (IProduct)new TestProduct(p))
                .ToList();
        }

        public IProduct GetProductById(int id)
        {
            if (_products.TryGetValue(id, out var productData))
            {
                return new TestProduct(productData);
            }
            return null;
        }

        public IEnumerable<IProduct> GetProductsByCategory(ProductCategory category)
        {
            return _products.Values
                .Where(p => p.Category == category)
                .Select(p => (IProduct)new TestProduct(p))
                .ToList();
        }

        public void UpdateProductStock(int productId, int newQuantity)
        {
            if (_products.TryGetValue(productId, out var productData))
            {
                productData.StockQuantity = newQuantity;
            }
        }

        // Struktura wewnętrzna do przechowywania danych produktu
        private class ProductData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public ProductCategory Category { get; set; }
        }

        private class TestProduct : IProduct
        {
            private readonly ProductData _data;

            public TestProduct(ProductData data)
            {
                _data = data;
            }

            public int Id => _data.Id;
            public string Name => _data.Name;
            public string Description => _data.Description;
            public decimal Price => _data.Price;
            public int StockQuantity => _data.StockQuantity;
            public ProductCategory Category => _data.Category;
        }
    }

    internal class UserServiceForTest : IUserService
    {
        private Dictionary<int, UserData> _users = new Dictionary<int, UserData>();
        private Dictionary<string, int> _emailIndex = new Dictionary<string, int>();
        private int _nextUserId = 1;

        public IUser GetUserById(int id)
        {
            if (_users.TryGetValue(id, out var userData))
            {
                return new TestUser(userData);
            }
            return null;
        }

        public IUser GetUserByEmail(string email)
        {
            if (_emailIndex.TryGetValue(email, out var userId) && _users.TryGetValue(userId, out var userData))
            {
                return new TestUser(userData);
            }
            return null;
        }

        public void RegisterUser(string name, string email, string address, string phoneNumber)
        {
            if (_emailIndex.ContainsKey(email))
            {
                throw new InvalidOperationException("Użytkownik o podanym adresie email już istnieje.");
            }

            var userData = new UserData
            {
                Id = _nextUserId,
                Name = name,
                Email = email,
                Address = address,
                PhoneNumber = phoneNumber,
                IsActive = true
            };

            _users.Add(_nextUserId, userData);
            _emailIndex.Add(email, _nextUserId);
            _nextUserId++;
        }

        public void UpdateUserStatus(int userId, bool isActive)
        {
            if (_users.TryGetValue(userId, out var userData))
            {
                userData.IsActive = isActive;
            }
        }

        // Struktura wewnętrzna do przechowywania danych użytkownika
        private class UserData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public bool IsActive { get; set; }
        }

        private class TestUser : IUser
        {
            private readonly UserData _data;

            public TestUser(UserData data)
            {
                _data = data;
            }

            public int Id => _data.Id;
            public string Name => _data.Name;
            public string Email => _data.Email;
            public string Address => _data.Address;
            public string PhoneNumber => _data.PhoneNumber;
        }
    }

    internal class ShoppingCartServiceForTest : IShoppingCartService
    {
        private Dictionary<int, Dictionary<int, int>> _userCarts = new Dictionary<int, Dictionary<int, int>>();
        private IProductService _productService;
        private IOrderService _orderService;

        public ShoppingCartServiceForTest(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        public void AddToCart(IUser user, IProduct product, int quantity)
        {
            if (!_userCarts.TryGetValue(user.Id, out var cart))
            {
                cart = new Dictionary<int, int>();
                _userCarts.Add(user.Id, cart);
            }

            if (cart.ContainsKey(product.Id))
            {
                cart[product.Id] += quantity;
            }
            else
            {
                cart.Add(product.Id, quantity);
            }
        }

        public void RemoveFromCart(IUser user, int productId)
        {
            if (_userCarts.TryGetValue(user.Id, out var cart) && cart.ContainsKey(productId))
            {
                cart.Remove(productId);
            }
        }

        public void ClearCart(IUser user)
        {
            if (_userCarts.ContainsKey(user.Id))
            {
                _userCarts[user.Id].Clear();
            }
        }

        public IOrder CreateOrderFromCart(IUser user, string description)
        {
            if (!_userCarts.TryGetValue(user.Id, out var cart) || cart.Count == 0)
            {
                throw new InvalidOperationException("Koszyk jest pusty.");
            }

            // Dostęp do wewnętrznej implementacji OrderService przez casting 
            var orderServiceImpl = _orderService as OrderServiceForTest;
            if (orderServiceImpl == null)
            {
                throw new InvalidOperationException("Niekompatybilna implementacja IOrderService");
            }

            int orderId = orderServiceImpl.CreateOrder(user, description);

            // Wyczyszczenie koszyka po utworzeniu zamówienia
            ClearCart(user);

            return _orderService.GetOrderById(orderId);
        }
    }
}