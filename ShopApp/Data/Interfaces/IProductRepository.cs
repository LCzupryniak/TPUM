using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        Product GetProductById(int productId);
        List<Product> GetAllProducts();
        void UpdateProduct(Product product);
        void RemoveProduct(int productId);
    }
}
