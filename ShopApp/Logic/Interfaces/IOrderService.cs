using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Logic.Interfaces
{
    public interface IOrderService
    {
        void ProcessOrder(IOrder order);
        IOrder GetOrderById(int id);
        IEnumerable<IOrder> GetOrdersByUser(IUser user);
        void UpdateOrderStatus(int orderId, OrderStatus newStatus);
        void CancelOrder(int orderId);
    }
}
