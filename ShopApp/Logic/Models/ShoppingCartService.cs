using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Logic.Interfaces;

namespace Logic.Models
{
    internal class ShoppingCartService : IShoppingCartService
    {
        private Dictionary<int, Dictionary<int, int>> _userCarts; // userId -> (productId -> quantity)
        private IProductService _productService;
        private IOrderService _orderService;

        public ShoppingCartService(IProductService productService, IOrderService orderService)
        {
            _userCarts = new Dictionary<int, Dictionary<int, int>>();
            _productService = productService;
            _orderService = orderService;
        }

        public void AddToCart(IUser user, IProduct product, int quantity)
        {
            if (!_userCarts.TryGetValue(user.Id, out var cart))
            {
                cart = new Dictionary<int, int>();
                _userCarts.Add(user.Id, cart);
            }

            if (cart.ContainsKey(product.Id))
            {
                cart[product.Id] += quantity;
            }
            else
            {
                cart.Add(product.Id, quantity);
            }
        }

        public void RemoveFromCart(IUser user, int productId)
        {
            if (_userCarts.TryGetValue(user.Id, out var cart) && cart.ContainsKey(productId))
            {
                cart.Remove(productId);
            }
        }

        public void ClearCart(IUser user)
        {
            if (_userCarts.ContainsKey(user.Id))
            {
                _userCarts[user.Id].Clear();
            }
        }

        public IOrder CreateOrderFromCart(IUser user, string description)
        {
            if (!_userCarts.TryGetValue(user.Id, out var cart) || cart.Count == 0)
            {
                throw new InvalidOperationException("Koszyk jest pusty.");
            }

            // Tworzenie zamówienia
            var orderService = (OrderService)_orderService;
            var orderId = orderService.CreateOrder((User)user, description);
            var order = (Order)_orderService.GetOrderById(orderId);

            // Dodawanie produktów z koszyka do zamówienia
            foreach (var item in cart)
            {
                var productId = item.Key;
                var quantity = item.Value;
                var product = (Product)((ConcreteProduct)_productService.GetProductById(productId)).GetInternalProduct();

                var orderItem = new OrderItem(productId, product, quantity);
                order.AddItem(orderItem);
            }

            // Wyczyszczenie koszyka po utworzeniu zamówienia
            ClearCart(user);

            return new ConcreteOrder(order.Id, order.Description, (User)order.Customer);
        }
    }
}
