using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public abstract class OrderBase : IOrder
    {
        public abstract int Id { get; }
        public abstract string Description { get; }
        public abstract decimal TotalAmount { get; }
        public abstract DateTime OrderDate { get; }
        public abstract IEnumerable<IOrderItem> Items { get; }
        public abstract IUser Customer { get; }
        public abstract OrderStatus Status { get; }
    }

    public abstract class OrderItemBase : IOrderItem
    {
        public abstract int Id { get; }
        public abstract IProduct Product { get; }
        public abstract int Quantity { get; }
        public abstract decimal UnitPrice { get; }
        public abstract decimal TotalPrice { get; }
    }

    public abstract class ProductBase : IProduct
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract decimal Price { get; }
        public abstract int StockQuantity { get; }
        public abstract ProductCategory Category { get; }
    }

    public abstract class UserBase : IUser
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Email { get; }
        public abstract string Address { get; }
        public abstract string PhoneNumber { get; }
        public abstract bool IsActive { get; }
    }
}
