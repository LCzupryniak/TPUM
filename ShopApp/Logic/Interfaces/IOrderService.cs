using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Logic.Interfaces
{
    public interface IOrderService
    {
        Order CreateNewOrder(Customer customer, List<Product> products);
        bool ProcessOrder(int orderId);
        decimal CalculateOrderTotal(List<Product> products);
        void UpdateOrderStatus(int orderId, OrderStatus newStatus);
    }
}

