using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Data.Models
{
    internal class Order : IOrder
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public decimal TotalAmount { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }

        private List<OrderItem> _items;
        public IEnumerable<IOrderItem> Items => _items.AsReadOnly();

        private User _customer;
        public IUser Customer => _customer;

        public Order(int id, string description, User customer, DateTime? orderDate = null)
        {
            Id = id;
            Description = description;
            _customer = customer;
            OrderDate = orderDate ?? DateTime.Now;
            Status = OrderStatus.New;
            _items = new List<OrderItem>();
            TotalAmount = 0;
        }

        public void AddItem(OrderItem item)
        {
            _items.Add(item);
            RecalculateTotalAmount();
        }

        public void RemoveItem(int itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _items.Remove(item);
                RecalculateTotalAmount();
            }
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = _items.Sum(item => item.TotalPrice);
        }
    }

    internal class OrderItem : IOrderItem
    {
        public int Id { get; private set; }
        private Product _product;
        public IProduct Product => _product;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public OrderItem(int id, Product product, int quantity)
        {
            Id = id;
            _product = product;
            Quantity = quantity;
            UnitPrice = product.Price;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity > 0)
            {
                Quantity = newQuantity;
            }
        }
    }
}
