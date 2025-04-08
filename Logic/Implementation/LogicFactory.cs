using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;
using Logic.API;

namespace Logic.Implementation
{
    public static class LogicFactory
    {
        public static IShopService CreateShopService(IDataRepository repository)
        {
            return new ShopService(repository); 
        }

        public static IProductStockNotifier CreateStockNotifier()
        {
            return new ProductStockNotifier(); 
        }
    }
}
