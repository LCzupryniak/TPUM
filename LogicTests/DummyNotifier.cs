using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Logic.API;

namespace LogicTests
{
    internal class DummyNotifier : IProductStockNotifier
    {
        private readonly System.Timers.Timer _timer;

        public event EventHandler StockChanged;

        public DummyNotifier()
        {
            _timer = new System.Timers.Timer(3000); 
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
