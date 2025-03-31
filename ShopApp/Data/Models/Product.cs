using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Data.Models
{
    internal class Product : IProduct
    {
    public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public ProductCategory Category { get; private set; }

        public Product(int id, string name, string description, decimal price, int stockQuantity, ProductCategory category)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            Category = category;
        }

        public void UpdateStock(int newQuantity)
        {
            StockQuantity = newQuantity;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice >= 0)
            {
                Price = newPrice;
            }
        }
    }
}

