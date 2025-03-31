using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Logic.Interfaces
{
    public interface IProductService
    {
        IEnumerable<IProduct> GetAllProducts();
        IProduct GetProductById(int id);
        IEnumerable<IProduct> GetProductsByCategory(ProductCategory category);
        void UpdateProductStock(int productId, int newQuantity);
    }
}
