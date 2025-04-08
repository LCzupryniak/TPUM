using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Logic.API;


namespace Logic.Implementation
{
    internal class ProductStockNotifier : IProductStockNotifier
    {
        private readonly System.Timers.Timer _timer;

        public event EventHandler StockChanged;

        public ProductStockNotifier()
        {
            _timer = new System.Timers.Timer(3000); // co 3 sekundy
            _timer.Elapsed += OnTimedEvent;
        }

        public void StartMonitoring()
        {
            _timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            StockChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetCurrentStock(string productName)
        {

            return new Random().Next(20, 100);
        }
        public void StopMonitoring()
        {
            _timer.Stop();
        }
    }
}
