using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        Order GetOrderById(int orderId);
        List<Order> GetOrdersByCustomer(int customerId);
    }
}
