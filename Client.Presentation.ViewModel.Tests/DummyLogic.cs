using Client.Presentation.Model.API;

namespace Client.Presentation.ViewModel.Tests
{
    internal class DummyCustomerModel : ICustomerModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public float Money { get; }
        public ICartModel Inventory { get; }

        public DummyCustomerModel(Guid id, string name, float money, ICartModel inventory)
        {
            Id = id;
            Name = name;
            Money = money;
            Inventory = inventory;
        }
    }

    internal class DummyInventoryModel : ICartModel
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductModel> Items { get; }

        public DummyInventoryModel(Guid id, int capacity, IEnumerable<IProductModel> items)
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
        private readonly ICartModelService _inventoryService;

        public DummyCustomerModelService(ICartModelService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void AddCustomer(Guid id, string name, float money, Guid inventoryId)
        {
            ICartModel? inventory = _inventoryService.GetInventory(inventoryId);
            if (inventory == null) throw new ArgumentException("Invalid inventory ID");
            _customers[id] = new DummyCustomerModel(id, name, money, inventory);
        }

        public IEnumerable<ICustomerModel> GetAllCustomers() => _customers.Values;

        public ICustomerModel? GetCustomer(Guid id) => _customers.GetValueOrDefault(id);

        public bool RemoveCustomer(Guid id) => _customers.Remove(id);

        public bool UpdateCustomer(Guid id, string name, float money, Guid inventoryId)
        {
            if (!_customers.ContainsKey(id)) return false;
            ICartModel? inventory = _inventoryService.GetInventory(inventoryId);
            if (inventory == null) return false;
            _customers[id] = new DummyCustomerModel(id, name, money, inventory);
            return true;
        }

        public void TriggerPeriodicItemMaintenanceDeduction()
        {
            foreach (ICustomerModel customer in _customers.Values)
            {
                float totalCost = customer.Inventory.Items.Sum(item => item.MaintenanceCost);
                _customers[customer.Id] = new DummyCustomerModel(customer.Id, customer.Name, Math.Max(0, customer.Money - totalCost), customer.Inventory);
            }
        }
    }

    internal class DummyInventoryModelService : ICartModelService
    {
        private readonly Dictionary<Guid, ICartModel> _inventories = new();

        public void Add(Guid id, int capacity)
        {
            _inventories[id] = new DummyInventoryModel(id, capacity, new List<IProductModel>());
        }

        public IEnumerable<ICartModel> GetAllInventories() => _inventories.Values;

        public ICartModel? GetInventory(Guid id) => _inventories.GetValueOrDefault(id);
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
                    _customerService.UpdateCustomer(order.Buyer.Id, order.Buyer.Name, order.Buyer.Money - totalCost, order.Buyer.Inventory.Id);
                    _orders.Remove(order.Id);
                }
            }
        }
    }
}