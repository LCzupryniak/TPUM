using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Logic.Interfaces
{
    public interface IShoppingCartService
    {
        void AddToCart(IUser user, IProduct product, int quantity);
        void RemoveFromCart(IUser user, int productId);
        void ClearCart(IUser user);
        IOrder CreateOrderFromCart(IUser user, string description);
    }
}
