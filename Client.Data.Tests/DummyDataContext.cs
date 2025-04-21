using Client.Data.API;
using ClientServer.Shared.Data.API;
using System;
using System.Collections.Generic;

namespace Client.Data.Tests
{
    internal class DummyDataContext : IDataContext
    {
        private readonly Dictionary<Guid, ICustomer> _customers = new Dictionary<Guid, ICustomer>();
        private readonly Dictionary<Guid, IProduct> _items = new Dictionary<Guid, IProduct>();
        private readonly Dictionary<Guid, ICart> _inventories = new Dictionary<Guid, ICart>();
        private readonly Dictionary<Guid, IOrder> _orders = new Dictionary<Guid, IOrder>();

        public Dictionary<Guid, ICustomer> Customers => _customers;
        public Dictionary<Guid, IProduct> Items => _items;
        public Dictionary<Guid, ICart> Inventories => _inventories;
        public Dictionary<Guid, IOrder> Orders => _orders;

        public event Action OnDataChanged = delegate { };

        
        public void TriggerDataChanged()
        {
            OnDataChanged?.Invoke(); //simulate the data changing in the context
        }

        public void ClearAll()
        {
            _customers.Clear();
            _items.Clear();
            _inventories.Clear();
            _orders.Clear();
        }
    }
}