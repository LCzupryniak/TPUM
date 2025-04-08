using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace Logic.API
{
    public interface IShopService
    {
        List<IProduct> GetAvailableProducts();
        bool PurchaseProduct(string productName);
    }
}
