using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.API;

namespace Model
{
    public class LogicModelBridge : IShopModelService, IProductStockModelNotifier
    {
        private readonly IShopService _shopService;
        private readonly IProductStockNotifier _notifier;

        public LogicModelBridge(IShopService shopService, IProductStockNotifier notifier)
        {
            _shopService = shopService;
            _notifier = notifier;
        }

        public void PurchaseProduct(string name) => _shopService.PurchaseProduct(name);

        public IEnumerable<IProductModel> GetAvailableProducts()
        {
            foreach (var product in _shopService.GetAvailableProducts())
            {
                yield return new ProductModel(product.Name, (decimal)product.Price);
            }
        }

        public int GetCurrentStock(string productName) => _notifier.GetCurrentStock(productName);
        public void StartMonitoring() => _notifier.StartMonitoring();
        public void StopMonitoring() => _notifier.StopMonitoring();
        public event EventHandler StockChanged
        {
            add { _notifier.StockChanged += value; }
            remove { _notifier.StockChanged -= value; }
        }

        private class ProductModel : IProductModel
        {
            public ProductModel(string name, decimal price)
            {
                Name = name;
                Price = price;
            }

            public string Name { get; }
            public decimal Price { get; }
        }
    }
}
