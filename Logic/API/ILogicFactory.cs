using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Implementation;

namespace Logic.API
{
    public interface ILogicFactory
    {
        IShopService CreateShopService();
        IProductStockNotifier CreateStockNotifier();
    }

    public static class LogicFactory
    {
        public static ILogicFactory CreateFactory()
        {
            return new LogicFactoryImpl();
        }
    }
}
