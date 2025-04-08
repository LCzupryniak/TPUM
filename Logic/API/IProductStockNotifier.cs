using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.API
{
    public interface IProductStockNotifier
    {
        event EventHandler StockChanged;
        int GetCurrentStock(string productName);
        void StartMonitoring();
        void StopMonitoring();
    }
}
