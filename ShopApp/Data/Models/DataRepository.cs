using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Data.Models
{
    internal class ConcreteOrder : OrderBase
    {
        private readonly Order _order;

        public ConcreteOrder(int id, string description, User customer, DateTime? orderDate = null)
        {
            _order = new Order(id, description, customer, orderDate);
        }

        public override int Id => _order.Id;
        public override string Description => _order.Description;
        public override decimal TotalAmount => _order.TotalAmount;
        public override DateTime OrderDate => _order.OrderDate;
        public override IEnumerable<IOrderItem> Items => _order.Items;
        public override IUser Customer => _order.Customer;
        public override OrderStatus Status => _order.Status;

        public void AddItem(ConcreteOrderItem item)
        {
            var internalItem = (OrderItem)((ConcreteOrderItem)item).GetInternalItem();
            _order.AddItem(internalItem);
        }

        public void UpdateStatus(OrderStatus status)
        {
            _order.UpdateStatus(status);
        }
    }

    internal class ConcreteOrderItem : OrderItemBase
    {
        private readonly OrderItem _orderItem;

        public ConcreteOrderItem(int id, ConcreteProduct product, int quantity)
        {
            var internalProduct = (Product)((ConcreteProduct)product).GetInternalProduct();
            _orderItem = new OrderItem(id, internalProduct, quantity);
        }

        public override int Id => _orderItem.Id;
        public override IProduct Product => _orderItem.Product;
        public override int Quantity => _orderItem.Quantity;
        public override decimal UnitPrice => _orderItem.UnitPrice;
        public override decimal TotalPrice => _orderItem.TotalPrice;

        public void UpdateQuantity(int newQuantity)
        {
            _orderItem.UpdateQuantity(newQuantity);
        }

        public object GetInternalItem()
        {
            return _orderItem;
        }
    }

    internal class ConcreteProduct : ProductBase
    {
        private readonly Product _product;

        public ConcreteProduct(int id, string name, string description, decimal price, int stockQuantity, ProductCategory category)
        {
            _product = new Product(id, name, description, price, stockQuantity, category);
        }

        public override int Id => _product.Id;
        public override string Name => _product.Name;
        public override string Description => _product.Description;
        public override decimal Price => _product.Price;
        public override int StockQuantity => _product.StockQuantity;
        public override ProductCategory Category => _product.Category;

        public void UpdateStock(int newQuantity)
        {
            _product.UpdateStock(newQuantity);
        }

        public void UpdatePrice(decimal newPrice)
        {
            _product.UpdatePrice(newPrice);
        }

        public object GetInternalProduct()
        {
            return _product;
        }
    }

    internal class ConcreteUser : UserBase
    {
        private readonly User _user;

        public ConcreteUser(int id, string name, string email, string address = "", string phoneNumber = "")
        {
            _user = new User(id, name, email, address, phoneNumber);
        }

        public override int Id => _user.Id;
        public override string Name => _user.Name;
        public override string Email => _user.Email;
        public override string Address => _user.Address;
        public override string PhoneNumber => _user.PhoneNumber;
        public override bool IsActive => _user.IsActive;

        public void UpdateContactInfo(string email, string address, string phoneNumber)
        {
            _user.UpdateContactInfo(email, address, phoneNumber);
        }

        public void SetActiveStatus(bool isActive)
        {
            _user.SetActiveStatus(isActive);
        }
    }
}
