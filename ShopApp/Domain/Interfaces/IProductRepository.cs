using System;
using System.Collections.Generic;

namespace ShopApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        Product GetProductById(int productId);
        List<Product> GetAllProducts();
        void UpdateProduct(Product product);
        void RemoveProduct(int productId);
    }

    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        Order GetOrderById(int orderId);
        List<Order> GetOrdersByCustomer(int customerId);
    }
}