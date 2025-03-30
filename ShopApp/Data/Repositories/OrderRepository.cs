using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;

namespace Data.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private List<Order> _orderStorage;
        private object _lockObject;

        public OrderRepository()
        {
            _orderStorage = new List<Order>();
            _lockObject = new object();
        }

        public void CreateOrder(Order order)
        {
            lock (_lockObject)
            {
                order.OrderId = _orderStorage.Count + 1;
                _orderStorage.Add(order);
            }
        }

        public Order GetOrderById(int orderId)
        {
            lock (_lockObject)
            {
                Order foundOrder = null;
                foreach (Order order in _orderStorage)
                {
                    if (order.OrderId == orderId)
                    {
                        foundOrder = order;
                        break;
                    }
                }
                return foundOrder;
            }
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            lock (_lockObject)
            {
                List<Order> customerOrders = new List<Order>();
                foreach (Order order in _orderStorage)
                {
                    if (order.CustomerOwner.CustomerId == customerId)
                    {
                        customerOrders.Add(order);
                    }
                }
                return customerOrders;
            }
        }
    }
}
