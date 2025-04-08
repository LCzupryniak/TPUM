using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTests
{
    [TestClass]
    public class DummyNotifierTests
    {
        private DummyNotifier _notifier;
        private bool _eventFired;

        [TestInitialize]
        public void Initialize()
        {
            _notifier = new DummyNotifier();
            _eventFired = false;
        }

        [TestMethod]
        public void GetCurrentStock_ReturnsValueBetween20And100()
        {
            // Act
            int stock = _notifier.GetCurrentStock("Dowolny produkt");

            // Assert
            Assert.IsTrue(stock >= 20 && stock <= 100);
        }

        [TestMethod]
        public void StockChanged_EventIsFired_AfterTimerElapsed()
        {
            // Arrange
            _notifier.StockChanged += HandleStockChanged;

            // Act
            _notifier.StartMonitoring();

            // Wait for the event to fire (timer is set to 3 seconds)
            Thread.Sleep(4000);

            // Stop monitoring
            _notifier.StopMonitoring();

            // Assert
            Assert.IsTrue(_eventFired);
        }

        [TestMethod]
        public void StopMonitoring_PreventsEventFromFiring()
        {
            // Arrange
            _notifier.StockChanged += HandleStockChanged;

            // Act
            _notifier.StartMonitoring();
            _notifier.StopMonitoring(); // natychmiast zatrzymujemy

            // Wait to ensure the event would have fired if not stopped
            Thread.Sleep(4000);

            // Assert
            Assert.IsFalse(_eventFired);
        }

        private void HandleStockChanged(object sender, EventArgs e)
        {
            _eventFired = true;
        }
    }
}
