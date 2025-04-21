using ClientServer.Shared.Data.API;

namespace Server.Logic.Tests
{
    internal class DummyDataRepository : IDataRepository
    {
        private readonly Dictionary<Guid, ICustomer> _customers = new Dictionary<Guid, ICustomer>();
        private readonly Dictionary<Guid, IProduct> _items = new Dictionary<Guid, IProduct>();
        private readonly Dictionary<Guid, ICart> _inventories = new Dictionary<Guid, ICart>();
        private readonly Dictionary<Guid, IOrder> _orders = new Dictionary<Guid, IOrder>();

        private readonly object _customersLock = new object();
        private readonly object _inventoryLock = new object();
        private readonly object _itemsLock = new object();
        private readonly object _ordersLock = new object();

        public event Action OnDataChanged = delegate { };

        public IEnumerable<ICustomer> GetAllCustomers()
        {
            lock (_customersLock)
            {
                return _customers.Values.ToList();
            }
        }

        public ICustomer? GetCustomer(Guid id)
        {
            lock (_customersLock)
            {
                if (_customers.ContainsKey(id))
                {
                    return _customers[id];
                }

                return null;
            }
        }

        public void AddCustomer(ICustomer customer)
        {
            lock (_customersLock)
            {
                _customers[customer.Id] = customer;
            }
        }

        public bool RemoveCustomerById(Guid id)
        {
            lock (_customersLock)
            {
                if (_customers.ContainsKey(id))
                {
                    _customers.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveCustomer(ICustomer customer)
        {
            lock (_customersLock)
            {
                if (_customers.ContainsKey(customer.Id))
                {
                    _customers.Remove(customer.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateCustomer(Guid id, ICustomer customer)
        {
            lock (_customersLock)
            {
                if (_customers.ContainsKey(id))
                {
                    _customers[id] = customer;
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<ICart> GetAllInventories()
        {
            lock (_inventoryLock)
            {
                return _inventories.Values.ToList();
            }
        }

        public ICart? GetInventory(Guid id)
        {
            lock (_inventoryLock)
            {
                if (_inventories.ContainsKey(id))
                {
                    return _inventories[id];
                }

                return null;
            }
        }

        public void AddInventory(ICart inventory)
        {
            lock (_inventoryLock)
            {
                _inventories[inventory.Id] = inventory;
            }
        }

        public bool RemoveInventoryById(Guid id)
        {
            lock (_inventoryLock)
            {
                if (_inventories.ContainsKey(id))
                {
                    _inventories.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveInventory(ICart inventory)
        {
            lock (_inventoryLock)
            {
                if (_inventories.ContainsKey(inventory.Id))
                {
                    _inventories.Remove(inventory.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateInventory(Guid id, ICart inventory)
        {
            lock (_inventoryLock)
            {
                if (_inventories.ContainsKey(id))
                {
                    _inventories[id] = inventory;
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<IProduct> GetAllItems()
        {
            lock (_itemsLock)
            {
                return _items.Values.ToList();
            }
        }

        public IProduct? GetItem(Guid id)
        {
            lock (_itemsLock)
            {
                if (_items.ContainsKey(id))
                {
                    return _items[id];
                }

                return null;
            }
        }

        public void AddItem(IProduct item)
        {
            lock (_itemsLock)
            {
                _items[item.Id] = item;
            }
        }

        public bool RemoveItemById(Guid id)
        {
            lock (_itemsLock)
            {
                if (_items.ContainsKey(id))
                {
                    _items.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveItem(IProduct item)
        {
            lock (_itemsLock)
            {
                if (_items.ContainsKey(item.Id))
                {
                    _items.Remove(item.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateItem(Guid id, IProduct item)
        {
            lock (_itemsLock)
            {
                if (_items.ContainsKey(id))
                {
                    _items[id] = item;
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<IOrder> GetAllOrders()
        {
            lock (_ordersLock)
            {
                return _orders.Values.ToList();
            }
        }

        public IOrder? GetOrder(Guid id)
        {
            lock (_ordersLock)
            {
                if (_orders.ContainsKey(id))
                {
                    return _orders[id];
                }

                return null;
            }
        }

        public void AddOrder(IOrder order)
        {
            lock (_ordersLock)
            {
                _orders[order.Id] = order;
            }
        }

        public bool RemoveOrderById(Guid id)
        {
            lock (_ordersLock)
            {
                if (_orders.ContainsKey(id))
                {
                    _orders.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveOrder(IOrder order)
        {
            lock (_ordersLock)
            {
                if (_orders.ContainsKey(order.Id))
                {
                    _orders.Remove(order.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateOrder(Guid id, IOrder order)
        {
            lock (_ordersLock)
            {
                if (_orders.ContainsKey(id))
                {
                    _orders[id] = order;
                    return true;
                }

                return false;
            }
        }
    }
}
