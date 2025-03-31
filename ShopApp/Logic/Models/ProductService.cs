using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Logic.Interfaces;

namespace Logic.Models
{
    internal class ProductService : IProductService
    {
        private Dictionary<int, Product> _products;

        public ProductService()
        {
            _products = new Dictionary<int, Product>();
            InitializeProductCatalog();
        }

        private void InitializeProductCatalog()
        {
            // Przykładowa inicjalizacja katalogu produktów
            AddProduct(new Product(1, "Telewizor LED 55\"", "Telewizor o wysokiej rozdzielczości", 2499.99m, 10, ProductCategory.Electronics));
            AddProduct(new Product(2, "Koszulka bawełniana", "Koszulka z czystej bawełny", 49.99m, 100, ProductCategory.Clothing));
            AddProduct(new Product(3, "Programowanie w C#", "Książka dla początkujących programistów", 89.99m, 25, ProductCategory.Books));
        }

        private void AddProduct(Product product)
        {
            _products.Add(product.Id, product);
        }

        public IEnumerable<IProduct> GetAllProducts()
        {
            return _products.Values
                .Select(p => new ConcreteProduct(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.Category))
                .ToList();
        }

        public IProduct GetProductById(int id)
        {
            if (_products.TryGetValue(id, out var product))
            {
                return new ConcreteProduct(product.Id, product.Name, product.Description, product.Price, product.StockQuantity, product.Category);
            }
            return null;
        }

        public IEnumerable<IProduct> GetProductsByCategory(ProductCategory category)
        {
            return _products.Values
                .Where(p => p.Category == category)
                .Select(p => new ConcreteProduct(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.Category))
                .ToList();
        }

        public void UpdateProductStock(int productId, int newQuantity)
        {
            if (_products.TryGetValue(productId, out var product))
            {
                product.UpdateStock(newQuantity);
            }
        }
    }

}
