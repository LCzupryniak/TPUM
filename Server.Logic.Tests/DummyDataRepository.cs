using Server.ObjectModels.Data.API;

namespace Server.Logic.Tests
{
    internal class DummyDataRepository : IDataRepository
    {
        private readonly Dictionary<Guid, ICustomer> _customers = new Dictionary<Guid, ICustomer>();
        private readonly Dictionary<Guid, IProduct> _items = new Dictionary<Guid, IProduct>();
        private readonly Dictionary<Guid, ICart> _carts = new Dictionary<Guid, ICart>();
        private readonly Dictionary<Guid, IOrder> _orders = new Dictionary<Guid, IOrder>();

        private readonly object _customersLock = new object();
        private readonly object _cartLock = new object();
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

        public IEnumerable<ICart> GetAllCarts()
        {
            lock (_cartLock)
            {
                return _carts.Values.ToList();
            }
        }

        public ICart? GetCart(Guid id)
        {
            lock (_cartLock)
            {
                if (_carts.ContainsKey(id))
                {
                    return _carts[id];
                }

                return null;
            }
        }

        public void AddCart(ICart cart)
        {
            lock (_cartLock)
            {
                _carts[cart.Id] = cart;
            }
        }

        public bool RemoveCartById(Guid id)
        {
            lock (_cartLock)
            {
                if (_carts.ContainsKey(id))
                {
                    _carts.Remove(id);
                    return true;
                }

                return false;
            }
        }

        public bool RemoveCart(ICart cart)
        {
            lock (_cartLock)
            {
                if (_carts.ContainsKey(cart.Id))
                {
                    _carts.Remove(cart.Id);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateCart(Guid id, ICart cart)
        {
            lock (_cartLock)
            {
                if (_carts.ContainsKey(id))
                {
                    _carts[id] = cart;
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
