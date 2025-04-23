using Client.ObjectModels.Data.API;

namespace Client.Data.Implementation
{
    internal class DataRepository : IDataRepository
    {
        private readonly IDataContext _context;

        private readonly object _customersLock = new object();
        private readonly object _cartLock = new object();
        private readonly object _itemsLock = new object();
        private readonly object _ordersLock = new object();

        public event Action OnDataChanged = delegate { };

        public DataRepository(IDataContext context)
        {
            this._context = context;

            this._context.OnDataChanged += () => OnDataChanged.Invoke();
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

        public IEnumerable<ICart> GetAllCarts()
        {
            lock (_cartLock)
            {
                return _context.Carts.Values.ToList();
            }
        }

        public ICart? GetCart(Guid id)
        {
            lock (_cartLock)
            {
                if (_context.Carts.ContainsKey(id))
                {
                    return _context.Carts[id];
                }

                return null;
            }
        }

        public void AddCart(ICart cart)
        {
            lock (_cartLock)
            {
                _context.Carts[cart.Id] = cart;
            }
        }

        public bool RemoveCartById(Guid id)
        {
            lock (_cartLock)
            {
                if (_context.Carts.ContainsKey(id))
                {
                    _context.Carts.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveCart(ICart cart)
        {
            lock (_cartLock)
            {
                if (_context.Carts.ContainsKey(cart.Id))
                {
                    _context.Carts.Remove(cart.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateCart(Guid id, ICart cart)
        {
            lock (_cartLock)
            {
                if (_context.Carts.ContainsKey(id))
                {
                    _context.Carts[id] = cart;
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
