using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Logic.Interfaces;

namespace Logic.Models
{
    internal class OrderService : IOrderService
    {
        private Dictionary<int, IOrder> _orders = new();
        private int _nextOrderId = 1;

        public void ProcessOrder(IOrder order)
        {
            order.UpdateStatus(OrderStatus.Processing);
        }

        public IOrder GetOrderById(int id)
        {
            return _orders.TryGetValue(id, out var order) ? order : null;
        }

        public IEnumerable<IOrder> GetOrdersByUser(IUser user)
        {
            return _orders.Values.Where(o => o.Customer.Id == user.Id).ToList();
        }

        public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            if (_orders.TryGetValue(orderId, out var order))
            {
                order.UpdateStatus(newStatus);
            }
        }

        public void CancelOrder(int orderId)
        {
            if (_orders.TryGetValue(orderId, out var order))
            {
                order.UpdateStatus(OrderStatus.Cancelled);
            }
        }

        public int CreateOrder(IUser customer, string description)
        {
            var newOrder = new ConcreteOrder(_nextOrderId, description, customer);
            _orders.Add(_nextOrderId, newOrder);
            return _nextOrderId++;
        }
    }
}
