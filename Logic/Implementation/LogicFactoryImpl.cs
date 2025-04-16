using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;
using Logic.API;

namespace Logic.Implementation
{
    public class LogicFactoryImpl : ILogicFactory
    {
        private readonly IDataRepository _repository;

        public LogicFactoryImpl()
        {
            _repository = DataFactory.CreateRepository(); // odwołanie do interfejsu API
        }

        public  IShopService CreateShopService()
        {
            return new ShopService(_repository);
        }

        public  IProductStockNotifier CreateStockNotifier()
        {
            return new ProductStockNotifier();
        }
    }
}
