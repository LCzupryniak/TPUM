using ClientServer.Shared.Data.API;

namespace Server.Data.Implementation
{
    internal class DataRepository : IDataRepository
    {
        private readonly IDataContext _context;

        private readonly object _customersLock = new object();
        private readonly object _inventoryLock = new object();
        private readonly object _itemsLock = new object();
        private readonly object _ordersLock = new object();

        public event Action OnDataChanged = delegate { };

        public DataRepository(IDataContext context)
        {
            this._context = context;
        }

        public IEnumerable<ICustomer> GetAllCustomers()
        {
            lock (_customersLock)
            {
                return _context.Customers.Values.ToList();
            }
        }

        public ICustomer? GetCustomer(Guid id)
        {
            lock (_customersLock)
            {
                if (_context.Customers.ContainsKey(id))
                {
                    return _context.Customers[id];
                }

                return null;
            }
        }

        public void AddCustomer(ICustomer customer)
        {
            lock (_customersLock)
            {
                _context.Customers[customer.Id] = customer;
            }
        }

        public bool RemoveCustomerById(Guid id)
        {
            lock (_customersLock)
            {
                if (_context.Customers.ContainsKey(id))
                {
                    _context.Customers.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveCustomer(ICustomer customer)
        {
            lock (_customersLock)
            {
                if (_context.Customers.ContainsKey(customer.Id))
                {
                    _context.Customers.Remove(customer.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateCustomer(Guid id, ICustomer customer)
        {
            lock (_customersLock)
            {
                if (_context.Customers.ContainsKey(id))
                {
                    _context.Customers[id] = customer;
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<ICart> GetAllInventories()
        {
            lock (_inventoryLock)
            {
                return _context.Inventories.Values.ToList();
            }
        }

        public ICart? GetInventory(Guid id)
        {
            lock (_inventoryLock)
            {
                if (_context.Inventories.ContainsKey(id))
                {
                    return _context.Inventories[id];
                }

                return null;
            }
        }

        public void AddInventory(ICart inventory)
        {
            lock (_inventoryLock)
            {
                _context.Inventories[inventory.Id] = inventory;
            }
        }

        public bool RemoveInventoryById(Guid id)
        {
            lock (_inventoryLock)
            {
                if (_context.Inventories.ContainsKey(id))
                {
                    _context.Inventories.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveInventory(ICart inventory)
        {
            lock (_inventoryLock)
            {
                if (_context.Inventories.ContainsKey(inventory.Id))
                {
                    _context.Inventories.Remove(inventory.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateInventory(Guid id, ICart inventory)
        {
            lock (_inventoryLock)
            {
                if (_context.Inventories.ContainsKey(id))
                {
                    _context.Inventories[id] = inventory;
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<IProduct> GetAllItems()
        {
            lock (_itemsLock)
            {
                return _context.Items.Values.ToList();
            }
        }

        public IProduct? GetItem(Guid id)
        {
            lock (_itemsLock)
            {
                if (_context.Items.ContainsKey(id))
                {
                    return _context.Items[id];
                }

                return null;
            }
        }

        public void AddItem(IProduct item)
        {
            lock (_itemsLock)
            {
                _context.Items[item.Id] = item;
            }
        }

        public bool RemoveItemById(Guid id)
        {
            lock (_itemsLock)
            {
                if (_context.Items.ContainsKey(id))
                {
                    _context.Items.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveItem(IProduct item)
        {
            lock (_itemsLock)
            {
                if (_context.Items.ContainsKey(item.Id))
                {
                    _context.Items.Remove(item.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateItem(Guid id, IProduct item)
        {
            lock (_itemsLock)
            {
                if (_context.Items.ContainsKey(id))
                {
                    _context.Items[id] = item;
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<IOrder> GetAllOrders()
        {
            lock (_ordersLock)
            {
                return _context.Orders.Values.ToList();
            }
        }

        public IOrder? GetOrder(Guid id)
        {
            lock (_ordersLock)
            {
                if (_context.Orders.ContainsKey(id))
                {
                    return _context.Orders[id];
                }

                return null;
            }
        }

        public void AddOrder(IOrder order)
        {
            lock (_ordersLock)
            {
                _context.Orders[order.Id] = order;
            }
        }

        public bool RemoveOrderById(Guid id)
        {
            lock (_ordersLock)
            {
                if (_context.Orders.ContainsKey(id))
                {
                    _context.Orders.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveOrder(IOrder order)
        {
            lock (_ordersLock)
            {
                if (_context.Orders.ContainsKey(order.Id))
                {
                    _context.Orders.Remove(order.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateOrder(Guid id, IOrder order)
        {
            lock (_ordersLock)
            {
                if (_context.Orders.ContainsKey(id))
                {
                    _context.Orders[id] = order;
                    return true;
                }

                return false;
            }
        }
    }
}
