using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUser
    {
        int Id { get; }
        string Name { get; }
        string Email { get; }
        string Address { get; }
        string PhoneNumber { get; }
        bool IsActive { get; }
    }

    public enum OrderStatus
    {
        New,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Books,
        HomeGoods,
        Groceries,
        Other
    }
}
