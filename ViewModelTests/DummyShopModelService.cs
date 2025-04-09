using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModelTests
{
    internal class DummyShopModelService : IShopModelService
    {
        public List<IProductModel> ProductsToReturn { get; set; } = new List<IProductModel>();
        public List<string> PurchasedProducts { get; set; } = new List<string>();

        public IEnumerable<IProductModel> GetAvailableProducts()
        {
            return ProductsToReturn;
        }

        public void PurchaseProduct(string productName)
        {
            PurchasedProducts.Add(productName);
        }
    }
}
