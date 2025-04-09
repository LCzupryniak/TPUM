using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public interface IProductStockModelNotifier
    {
        event EventHandler StockChanged;
        void StartMonitoring();
        void StopMonitoring();
        int GetCurrentStock(string productName);
    }
}
