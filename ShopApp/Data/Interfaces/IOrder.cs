using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IOrder
    {
        int Id { get; }
        string Description { get; }
        decimal TotalAmount { get; }
        DateTime OrderDate { get; }
        IEnumerable<IOrderItem> Items { get; }
        IUser Customer { get; }
        OrderStatus Status { get; }
    }

    public interface IOrderItem
    {
        int Id { get; }
        IProduct Product { get; }
        int Quantity { get; }
        decimal UnitPrice { get; }
        decimal TotalPrice { get; }
    }
}
