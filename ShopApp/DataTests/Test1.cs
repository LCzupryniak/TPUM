using System;
using System.Collections.Generic;
using System.Linq;
using Data.Interfaces;

// Zakładamy, że klasy modelu znajdują się w przestrzeni nazw ShopSystem.DataLayer
// Należy dostosować nazwę przestrzeni nazw do faktycznej struktury projektu
namespace DataTests.Tests
{
    // Korzystamy z wewnętrznych klas implementacyjnych poprzez fabryki
    public class DataLayerFactory
    {
        // Fabryka do tworzenia produktów
        public static ProductBase CreateProduct(int id, string name, string description, decimal price, int stockQuantity, ProductCategory category)
        {
            // Tworzymy instancję ConcreteProduct przez refleksję lub fabrykę systemu
            Type productType = Type.GetType("ShopSystem.DataLayer.ConcreteProduct, ShopSystem.DataLayer");
            return (ProductBase)Activator.CreateInstance(productType, id, name, description, price, stockQuantity, category);
        }

        // Fabryka do tworzenia użytkowników
        public static UserBase CreateUser(int id, string name, string email, string address = "", string phoneNumber = "")
        {
            Type userType = Type.GetType("ShopSystem.DataLayer.ConcreteUser, ShopSystem.DataLayer");
            return (UserBase)Activator.CreateInstance(userType, id, name, email, address, phoneNumber);
        }

        // Fabryka do tworzenia elementów zamówienia
        public static OrderItemBase CreateOrderItem(int id, ProductBase product, int quantity)
        {
            Type orderItemType = Type.GetType("ShopSystem.DataLayer.ConcreteOrderItem, ShopSystem.DataLayer");
            return (OrderItemBase)Activator.CreateInstance(orderItemType, id, product, quantity);
        }

        // Fabryka do tworzenia zamówień
        public static OrderBase CreateOrder(int id, string description, UserBase customer, DateTime? orderDate = null)
        {
            Type orderType = Type.GetType("ShopSystem.DataLayer.ConcreteOrder, ShopSystem.DataLayer");
            return (OrderBase)Activator.CreateInstance(orderType, id, description, customer, orderDate);
        }
    }

    // Interfejs do operacji na produktach
    public interface IProductOperations
    {
        void UpdateStock(int newQuantity);
        void UpdatePrice(decimal newPrice);
    }

    // Interfejs do operacji na elementach zamówienia
    public interface IOrderItemOperations
    {
        void UpdateQuantity(int newQuantity);
    }

    // Interfejs do operacji na zamówieniach
    public interface IOrderOperations
    {
        void AddItem(OrderItemBase item);
        void UpdateStatus(OrderStatus status);
    }

    // Interfejs do operacji na użytkownikach
    public interface IUserOperations
    {
        void UpdateContactInfo(string email, string address, string phoneNumber);
        void SetActiveStatus(bool isActive);
    }

    // Klasa testująca jednostkowo warstwę danych
    public class DataLayerTests
    {
        // Test tworzenia i właściwości produktu
        public void TestProductCreationAndProperties()
        {
            // Arrange
            int id = 1;
            string name = "Laptop";
            string description = "Powerful gaming laptop";
            decimal price = 3999.99m;
            int stockQuantity = 10;
            ProductCategory category = ProductCategory.Electronics;

            // Act
            ProductBase product = DataLayerFactory.CreateProduct(id, name, description, price, stockQuantity, category);

            // Assert
            Assert.AreEqual(id, product.Id);
            Assert.AreEqual(name, product.Name);
            Assert.AreEqual(description, product.Description);
            Assert.AreEqual(price, product.Price);
            Assert.AreEqual(stockQuantity, product.StockQuantity);
            Assert.AreEqual(category, product.Category);

            Console.WriteLine("TestProductCreationAndProperties: PASSED");
        }

        // Test aktualizacji stanu magazynowego produktu
        public void TestProductStockUpdate()
        {
            // Arrange
            ProductBase product = DataLayerFactory.CreateProduct(1, "Laptop", "Description", 1999.99m, 10, ProductCategory.Electronics);
            int newStockQuantity = 5;

            // Act
            IProductOperations productOps = (IProductOperations)product;
            productOps.UpdateStock(newStockQuantity);

            // Assert
            Assert.AreEqual(newStockQuantity, product.StockQuantity);

            Console.WriteLine("TestProductStockUpdate: PASSED");
        }

        // Test aktualizacji ceny produktu
        public void TestProductPriceUpdate()
        {
            // Arrange
            ProductBase product = DataLayerFactory.CreateProduct(1, "Laptop", "Description", 1999.99m, 10, ProductCategory.Electronics);
            decimal newPrice = 1799.99m;

            // Act
            IProductOperations productOps = (IProductOperations)product;
            productOps.UpdatePrice(newPrice);

            // Assert
            Assert.AreEqual(newPrice, product.Price);

            // Test z nieprawidłową ceną (ujemną) - nie powinna zostać zaktualizowana
            productOps.UpdatePrice(-100m);
            Assert.AreEqual(newPrice, product.Price);

            Console.WriteLine("TestProductPriceUpdate: PASSED");
        }

        // Test tworzenia i właściwości użytkownika
        public void TestUserCreationAndProperties()
        {
            // Arrange
            int id = 1;
            string name = "Jan Kowalski";
            string email = "jan.kowalski@example.com";
            string address = "ul. Przykładowa 123, 00-000 Warszawa";
            string phoneNumber = "+48 123 456 789";

            // Act
            UserBase user = DataLayerFactory.CreateUser(id, name, email, address, phoneNumber);

            // Assert
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(name, user.Name);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(address, user.Address);
            Assert.AreEqual(phoneNumber, user.PhoneNumber);
            Assert.AreEqual(true, user.IsActive); // Domyślnie użytkownik jest aktywny

            Console.WriteLine("TestUserCreationAndProperties: PASSED");
        }

        // Test aktualizacji danych kontaktowych użytkownika
        public void TestUserContactInfoUpdate()
        {
            // Arrange
            UserBase user = DataLayerFactory.CreateUser(1, "Jan Kowalski", "old@example.com");
            string newEmail = "new@example.com";
            string newAddress = "ul. Nowa 456, 00-000 Warszawa";
            string newPhone = "+48 987 654 321";

            // Act
            IUserOperations userOps = (IUserOperations)user;
            userOps.UpdateContactInfo(newEmail, newAddress, newPhone);

            // Assert
            Assert.AreEqual(newEmail, user.Email);
            Assert.AreEqual(newAddress, user.Address);
            Assert.AreEqual(newPhone, user.PhoneNumber);

            Console.WriteLine("TestUserContactInfoUpdate: PASSED");
        }

        // Test zmiany statusu aktywności użytkownika
        public void TestUserActiveStatusChange()
        {
            // Arrange
            UserBase user = DataLayerFactory.CreateUser(1, "Jan Kowalski", "jan.kowalski@example.com");

            // Act
            IUserOperations userOps = (IUserOperations)user;
            userOps.SetActiveStatus(false);

            // Assert
            Assert.AreEqual(false, user.IsActive);

            // Przywrócenie aktywności
            userOps.SetActiveStatus(true);
            Assert.AreEqual(true, user.IsActive);

            Console.WriteLine("TestUserActiveStatusChange: PASSED");
        }

        // Test tworzenia elementu zamówienia i jego właściwości
        public void TestOrderItemCreationAndProperties()
        {
            // Arrange
            int id = 1;
            ProductBase product = DataLayerFactory.CreateProduct(1, "Laptop", "Description", 1999.99m, 10, ProductCategory.Electronics);
            int quantity = 2;

            // Act
            OrderItemBase orderItem = DataLayerFactory.CreateOrderItem(id, product, quantity);

            // Assert
            Assert.AreEqual(id, orderItem.Id);
            Assert.AreEqual(product.Id, orderItem.Product.Id);
            Assert.AreEqual(quantity, orderItem.Quantity);
            Assert.AreEqual(product.Price, orderItem.UnitPrice);
            Assert.AreEqual(product.Price * quantity, orderItem.TotalPrice);

            Console.WriteLine("TestOrderItemCreationAndProperties: PASSED");
        }

        // Test aktualizacji ilości w elemencie zamówienia
        public void TestOrderItemQuantityUpdate()
        {
            // Arrange
            ProductBase product = DataLayerFactory.CreateProduct(1, "Laptop", "Description", 1999.99m, 10, ProductCategory.Electronics);
            OrderItemBase orderItem = DataLayerFactory.CreateOrderItem(1, product, 2);
            int newQuantity = 3;

            // Act
            IOrderItemOperations orderItemOps = (IOrderItemOperations)orderItem;
            orderItemOps.UpdateQuantity(newQuantity);

            // Assert
            Assert.AreEqual(newQuantity, orderItem.Quantity);
            Assert.AreEqual(product.Price * newQuantity, orderItem.TotalPrice);

            // Test z nieprawidłową ilością (0 lub mniej) - nie powinna zostać zaktualizowana
            orderItemOps.UpdateQuantity(0);
            Assert.AreEqual(newQuantity, orderItem.Quantity); // Ilość nie powinna się zmienić

            Console.WriteLine("TestOrderItemQuantityUpdate: PASSED");
        }

        // Test tworzenia zamówienia i jego właściwości
        public void TestOrderCreationAndProperties()
        {
            // Arrange
            int id = 1;
            string description = "Zamówienie testowe";
            UserBase customer = DataLayerFactory.CreateUser(1, "Jan Kowalski", "jan.kowalski@example.com");
            DateTime orderDate = new DateTime(2023, 1, 15);

            // Act
            OrderBase order = DataLayerFactory.CreateOrder(id, description, customer, orderDate);

            // Assert
            Assert.AreEqual(id, order.Id);
            Assert.AreEqual(description, order.Description);
            Assert.AreEqual(orderDate, order.OrderDate);
            Assert.AreEqual(OrderStatus.New, order.Status);
            Assert.AreEqual(customer.Id, order.Customer.Id);
            Assert.AreEqual(0, order.Items.Count());
            Assert.AreEqual(0m, order.TotalAmount);

            Console.WriteLine("TestOrderCreationAndProperties: PASSED");
        }

        // Test dodawania elementów do zamówienia
        public void TestAddingItemsToOrder()
        {
            // Arrange
            UserBase customer = DataLayerFactory.CreateUser(1, "Jan Kowalski", "jan.kowalski@example.com");
            OrderBase order = DataLayerFactory.CreateOrder(1, "Zamówienie testowe", customer);

            ProductBase product1 = DataLayerFactory.CreateProduct(1, "Laptop", "Description", 1999.99m, 10, ProductCategory.Electronics);
            ProductBase product2 = DataLayerFactory.CreateProduct(2, "Mysz", "Description", 99.99m, 20, ProductCategory.Electronics);

            OrderItemBase item1 = DataLayerFactory.CreateOrderItem(1, product1, 1);
            OrderItemBase item2 = DataLayerFactory.CreateOrderItem(2, product2, 2);

            // Act
            IOrderOperations orderOps = (IOrderOperations)order;
            orderOps.AddItem(item1);
            orderOps.AddItem(item2);

            // Assert
            Assert.AreEqual(2, order.Items.Count());
            Assert.AreEqual(product1.Price + (product2.Price * 2), order.TotalAmount);

            Console.WriteLine("TestAddingItemsToOrder: PASSED");
        }

        // Test aktualizacji statusu zamówienia
        public void TestOrderStatusUpdate()
        {
            // Arrange
            UserBase customer = DataLayerFactory.CreateUser(1, "Jan Kowalski", "jan.kowalski@example.com");
            OrderBase order = DataLayerFactory.CreateOrder(1, "Zamówienie testowe", customer);

            // Act
            IOrderOperations orderOps = (IOrderOperations)order;
            orderOps.UpdateStatus(OrderStatus.Processing);

            // Assert
            Assert.AreEqual(OrderStatus.Processing, order.Status);

            // Kolejna zmiana statusu
            orderOps.UpdateStatus(OrderStatus.Shipped);
            Assert.AreEqual(OrderStatus.Shipped, order.Status);

            Console.WriteLine("TestOrderStatusUpdate: PASSED");
        }

        // Test domyślnej daty zamówienia (powinna być aktualna)
        public void TestOrderDefaultDate()
        {
            // Arrange
            UserBase customer = DataLayerFactory.CreateUser(1, "Jan Kowalski", "jan.kowalski@example.com");
            DateTime before = DateTime.Now.AddSeconds(-1);

            // Act
            OrderBase order = DataLayerFactory.CreateOrder(1, "Zamówienie testowe", customer);
            DateTime after = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.IsTrue(order.OrderDate >= before && order.OrderDate <= after);

            Console.WriteLine("TestOrderDefaultDate: PASSED");
        }

        // Test kompletnego procesu tworzenia zamówienia z wieloma elementami
        public void TestCompleteOrderProcess()
        {
            // Arrange
            UserBase customer = DataLayerFactory.CreateUser(1, "Jan Kowalski", "jan.kowalski@example.com", "ul. Przykładowa 123", "+48 123 456 789");

            ProductBase product1 = DataLayerFactory.CreateProduct(1, "Laptop", "Opis laptopa", 3999.99m, 5, ProductCategory.Electronics);
            ProductBase product2 = DataLayerFactory.CreateProduct(2, "Myszka", "Opis myszki", 199.99m, 15, ProductCategory.Electronics);
            ProductBase product3 = DataLayerFactory.CreateProduct(3, "Klawiatura", "Opis klawiatury", 299.99m, 10, ProductCategory.Electronics);

            OrderBase order = DataLayerFactory.CreateOrder(1, "Zamówienie sprzętu komputerowego", customer);
            IOrderOperations orderOps = (IOrderOperations)order;

            // Act
            orderOps.AddItem(DataLayerFactory.CreateOrderItem(1, product1, 1));
            orderOps.AddItem(DataLayerFactory.CreateOrderItem(2, product2, 2));
            orderOps.AddItem(DataLayerFactory.CreateOrderItem(3, product3, 1));

            // Przejście przez cykl życia zamówienia
            orderOps.UpdateStatus(OrderStatus.Processing);
            orderOps.UpdateStatus(OrderStatus.Shipped);
            orderOps.UpdateStatus(OrderStatus.Delivered);

            // Assert
            Assert.AreEqual(3, order.Items.Count());
            Assert.AreEqual(OrderStatus.Delivered, order.Status);

            decimal expectedTotal = product1.Price + (product2.Price * 2) + product3.Price;
            Assert.AreEqual(expectedTotal, order.TotalAmount);

            Console.WriteLine("TestCompleteOrderProcess: PASSED");
        }

        // Uruchomienie wszystkich testów
        public void RunAllTests()
        {
            TestProductCreationAndProperties();
            TestProductStockUpdate();
            TestProductPriceUpdate();
            TestUserCreationAndProperties();
            TestUserContactInfoUpdate();
            TestUserActiveStatusChange();
            TestOrderItemCreationAndProperties();
            TestOrderItemQuantityUpdate();
            TestOrderCreationAndProperties();
            TestAddingItemsToOrder();
            TestOrderStatusUpdate();
            TestOrderDefaultDate();
            TestCompleteOrderProcess();

            Console.WriteLine("All tests completed successfully!");
        }
    }

    // Prosta implementacja klasy Assert do testów jednostkowych
    public static class Assert
    {
        public static void AreEqual(object expected, object actual)
        {
            if (!object.Equals(expected, actual))
            {
                throw new AssertFailedException($"Expected: {expected}, but was: {actual}");
            }
        }

        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                throw new AssertFailedException("Expected condition to be true, but was false");
            }
        }
    }

    // Własna klasa wyjątku dla błędów asercji
    public class AssertFailedException : Exception
    {
        public AssertFailedException(string message) : base(message)
        {
        }
    }

    // Główna klasa do uruchomienia testów
    public class Program
    {
        public static void Main()
        {
            DataLayerTests tests = new DataLayerTests();
            tests.RunAllTests();
        }
    }
}