using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModelTests
{
    internal class DummyProductStockModelNotifier : IProductStockModelNotifier
    {
        public Dictionary<string, int> Stock { get; set; } = new Dictionary<string, int>();

        public event EventHandler StockChanged;

        public int GetCurrentStock(string productName)
        {
            return Stock.ContainsKey(productName) ? Stock[productName] : 0;
        }

        public void StartMonitoring() { }

        public void StopMonitoring() { }

        public void RaiseStockChanged()
        {
            StockChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
