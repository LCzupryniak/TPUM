using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.API;
using Data.API;

namespace LogicTests
{
    internal class DummyShopService : IShopService
    {
        private IDataRepository _repository;
        private Dictionary<string, int> _stock;

        public DummyShopService(IDataRepository repository)
        {
            _repository = repository;
            _stock = new Dictionary<string, int>();

            foreach (IProduct product in _repository.GetAllProducts())
            {
                _stock[product.Name] = 10; // początkowy stan magazynu
            }
        }

        public List<IProduct> GetAvailableProducts()
        {
            return _repository.GetAllProducts();
        }

        public bool PurchaseProduct(string productName)
        {
            if (_stock.ContainsKey(productName) && _stock[productName] > 0)
            {
                _stock[productName]--;
                return true;
            }
            return false;
        }

        public int GetStock(string productName)
        {
            if (_stock.ContainsKey(productName))
            {
                return _stock[productName];
            }
            return 0;
        }
    }
}

