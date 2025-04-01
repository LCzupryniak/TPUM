using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Interfaces;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private OrderModel _selectedOrder;
        private List<OrderModel> _orders;

        public OrderModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        public List<OrderModel> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged("Orders");
            }
        }

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            _orders = new List<OrderModel>();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // W rzeczywistej aplikacji tutaj pobieralibyśmy dane z serwisu
            SelectedOrder = new OrderModel
            {
                Id = 1,
                Description = "Zamówienie przykładowe",
                TotalAmount = 2639.97m,
                OrderDate = DateTime.Now,
                CustomerName = "Jan Kowalski",
                Status = "Nowe",
                Items = new List<OrderItemModel>
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
            }
            };

            Orders = new List<OrderModel> { SelectedOrder };
        }

        public void ProcessOrder(int orderId)
        {
            // Implementacja akcji przetwarzania zamówienia
            // W rzeczywistej aplikacji wywoływalibyśmy tu metodę serwisu
        }

        public void CancelOrder(int orderId)
        {
            // Implementacja akcji anulowania zamówienia
            // W rzeczywistej aplikacji wywoływalibyśmy tu metodę serwisu
        }
    }

}
