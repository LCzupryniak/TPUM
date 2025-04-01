using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Interfaces;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class ShoppingCartViewModel : ViewModelBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IOrderService _orderService;
        private List<OrderItemModel> _cartItems;
        private decimal _totalAmount;

        public List<OrderItemModel> CartItems
        {
            get { return _cartItems; }
            set
            {
                _cartItems = value;
                OnPropertyChanged("CartItems");
                CalculateTotalAmount();
            }
        }

        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        public ShoppingCartViewModel(IShoppingCartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
            _cartItems = new List<OrderItemModel>();
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            CartItems = new List<OrderItemModel>
        {
            new OrderItemModel
            {
                Id = 1,
                ProductName = "Telewizor LED 55\"",
                ProductDescription = "Telewizor o wysokiej rozdzielczości",
                Quantity = 1,
                UnitPrice = 2499.99m,
                TotalPrice = 2499.99m
            },
            new OrderItemModel
            {
                Id = 2,
                ProductName = "Koszulka bawełniana",
                ProductDescription = "Koszulka z czystej bawełny",
                Quantity = 2,
                UnitPrice = 49.99m,
                TotalPrice = 99.98m
            }
        };
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = CartItems.Sum(item => item.TotalPrice);
        }

        public void UpdateItemQuantity(int itemId, int newQuantity)
        {

        }

        public void RemoveItem(int itemId)
        {

        }

        public void Checkout()
        {

        }
    }
}
