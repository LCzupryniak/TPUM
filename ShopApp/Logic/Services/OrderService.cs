using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Logic.Interfaces;



namespace Logic.Services
{
    internal class OrderService : IOrderService
    {
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        //public OrderProcessingService(IProductRepository productRepo, IOrderRepository orderRepo)
        //{
        //    _productRepository = productRepo;
        //    _orderRepository = orderRepo;
        //}

        public Order CreateNewOrder(Customer customer, List<Product> products)
        {
            Order newOrder = new Order();
            newOrder.CustomerOwner = customer;
            newOrder.OrderedProducts = products;
            newOrder.TotalCost = CalculateOrderTotal(products);

            _orderRepository.CreateOrder(newOrder);
            return newOrder;
        }

        public bool ProcessOrder(int orderId)
        {
            Order order = _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return false;
            }

            UpdateOrderStatus(orderId, OrderStatus.Processing);
            return true;
        }

        public decimal CalculateOrderTotal(List<Product> products)
        {
            decimal totalCost = 0;
            foreach (Product product in products)
            {
                totalCost += product.Price;
            }
            return totalCost;
        }

        public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            Order order = _orderRepository.GetOrderById(orderId);
            if (order != null)
            {
                order.CurrentStatus = newStatus;
            }
        }
    }
}
