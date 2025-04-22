using Client.Presentation.Model.API;

namespace Client.Presentation.ViewModel.Tests
{
    internal class DummyCustomerModel : ICustomerModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public float Money { get; }
        public ICartModel Cart { get; }

        public DummyCustomerModel(Guid id, string name, float money, ICartModel cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }

    internal class DummyCartModel : ICartModel
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductModel> Items { get; }

        public DummyCartModel(Guid id, int capacity, IEnumerable<IProductModel> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items ?? Enumerable.Empty<IProductModel>();
        }
    }

    internal class DummyItemModel : IProductModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public DummyItemModel(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }

    internal class DummyOrderModel : IOrderModel
    {
        public Guid Id { get; }
        public ICustomerModel Buyer { get; }
        public IEnumerable<IProductModel> ItemsToBuy { get; }

        public DummyOrderModel(Guid id, ICustomerModel buyer, IEnumerable<IProductModel> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy?.ToList() ?? new List<IProductModel>();
        }
    }

    internal class DummyCustomerModelService : ICustomerModelService
    {
        private readonly Dictionary<Guid, ICustomerModel> _customers = new();
        private readonly ICartModelService _cartService;

        public DummyCustomerModelService(ICartModelService cartService)
        {
            _cartService = cartService;
        }

        public void AddCustomer(Guid id, string name, float money, Guid cartId)
        {
            ICartModel? cart = _cartService.GetCart(cartId);
            if (cart == null) throw new ArgumentException("Invalid cart ID");
            _customers[id] = new DummyCustomerModel(id, name, money, cart);
        }

        public IEnumerable<ICustomerModel> GetAllCustomers() => _customers.Values;

        public ICustomerModel? GetCustomer(Guid id) => _customers.GetValueOrDefault(id);

        public bool RemoveCustomer(Guid id) => _customers.Remove(id);

        public bool UpdateCustomer(Guid id, string name, float money, Guid cartId)
        {
            if (!_customers.ContainsKey(id)) return false;
            ICartModel? cart = _cartService.GetCart(cartId);
            if (cart == null) return false;
            _customers[id] = new DummyCustomerModel(id, name, money, cart);
            return true;
        }

        public void TriggerPeriodicItemMaintenanceDeduction()
        {
            foreach (ICustomerModel customer in _customers.Values)
            {
                float totalCost = customer.Cart.Items.Sum(item => item.MaintenanceCost);
                _customers[customer.Id] = new DummyCustomerModel(customer.Id, customer.Name, Math.Max(0, customer.Money - totalCost), customer.Cart);
            }
        }
    }

    internal class DummyCartModelService : ICartModelService
    {
        private readonly Dictionary<Guid, ICartModel> _carts = new();

        public void Add(Guid id, int capacity)
        {
            _carts[id] = new DummyCartModel(id, capacity, new List<IProductModel>());
        }

        public IEnumerable<ICartModel> GetAllCarts() => _carts.Values;

        public ICartModel? GetCart(Guid id) => _carts.GetValueOrDefault(id);
    }

    internal class DummyItemModelService : IProductModelService
    {
        private readonly Dictionary<Guid, IProductModel> _items = new();

        public void AddItem(Guid id, string name, int price, int maintenanceCost)
        {
            _items[id] = new DummyItemModel(id, name, price, maintenanceCost);
        }

        public IEnumerable<IProductModel> GetAllItems() => _items.Values;

        public IProductModel? GetItem(Guid id) => _items.GetValueOrDefault(id);

        public bool RemoveItem(Guid id) => _items.Remove(id);

        public bool UpdateItem(Guid id, string name, int price, int maintenanceCost)
        {
            if (!_items.ContainsKey(id)) return false;
            _items[id] = new DummyItemModel(id, name, price, maintenanceCost);
            return true;
        }
    }

    internal class DummyOrderModelService : IOrderModelService
    {
        private readonly Dictionary<Guid, IOrderModel> _orders = new();
        private readonly ICustomerModelService _customerService;
        private readonly IProductModelService _itemService;

        public DummyOrderModelService(ICustomerModelService customerService, IProductModelService itemService)
        {
            _customerService = customerService;
            _itemService = itemService;
        }

        public void AddOrder(Guid id, Guid buyerId, IEnumerable<Guid> itemIds)
        {
            ICustomerModel? buyer = _customerService.GetCustomer(buyerId);
            if (buyer == null) throw new ArgumentException("Invalid buyer ID");

            List<IProductModel?> itemsToBuy = itemIds.Select(id => _itemService.GetItem(id)).Where(item => item != null).ToList();
            if (!itemsToBuy.Any()) throw new ArgumentException("No valid items to buy");

            _orders[id] = new DummyOrderModel(id, buyer, itemsToBuy!);
        }

        public IEnumerable<IOrderModel> GetAllOrders() => _orders.Values;

        public IOrderModel? GetOrder(Guid id) => _orders.GetValueOrDefault(id);

        public bool RemoveOrder(Guid id) => _orders.Remove(id);

        public void TriggerPeriodicOrderProcessing()
        {
            foreach (IOrderModel order in _orders.Values.ToList())
            {
                float totalCost = order.ItemsToBuy.Sum(item => item.Price);
                if (order.Buyer.Money >= totalCost)
                {
                    _customerService.UpdateCustomer(order.Buyer.Id, order.Buyer.Name, order.Buyer.Money - totalCost, order.Buyer.Cart.Id);
                    _orders.Remove(order.Id);
                }
            }
        }
    }
}