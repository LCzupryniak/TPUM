using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Tests
{
    // Dummy implementation of ICustomerLogic
    internal class DummyCustomerLogic : ICustomerLogic
    {
        internal readonly Dictionary<Guid, ICustomerDataTransferObject> Customers = new();
        public int PeriodicDeductionCallCount { get; private set; } = 0;

        public void Add(ICustomerDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Customers[item.Id] = item;
        }

        public IEnumerable<ICustomerDataTransferObject> GetAll()
        {
            return Customers.Values.ToList();
        }

        public ICustomerDataTransferObject? Get(Guid id)
        {
            Customers.TryGetValue(id, out ICustomerDataTransferObject? customer);
            return customer;
        }

        public void PeriodicItemMaintenanceDeduction()
        {
            PeriodicDeductionCallCount++;
        }

        public void DeduceMaintenanceCost(ICustomerDataTransferObject customer)
        {
            foreach (IProductDataTransferObject item in customer.Inventory.Items)
            {
                customer.Money -= item.MaintenanceCost;
            }
        }

        public bool Remove(ICustomerDataTransferObject item)
        {
            if (item == null) return false;
            return Customers.Remove(item.Id);
        }

        public bool RemoveById(Guid id)
        {
            return Customers.Remove(id);
        }

        public bool Update(Guid id, ICustomerDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Customers.ContainsKey(id))
            {
                Customers[id] = item;
                return true;
            }
            return false;
        }
    }


    // Dummy implementation of IInventoryLogic
    internal class DummyInventoryLogic : ICartLogic
    {
        internal readonly Dictionary<Guid, IInventoryDataTransferObject> Inventories = new();

        public void Add(IInventoryDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Inventories[item.Id] = item;
        }

        public IEnumerable<IInventoryDataTransferObject> GetAll()
        {
            return Inventories.Values.ToList();
        }

        public IInventoryDataTransferObject? Get(Guid id)
        {
            Inventories.TryGetValue(id, out IInventoryDataTransferObject? inventory);
            return inventory;
        }

        public bool Remove(IInventoryDataTransferObject item)
        {
            if (item == null) return false;
            return Inventories.Remove(item.Id);
        }

        public bool RemoveById(Guid id)
        {
            return Inventories.Remove(id);
        }

        public bool Update(Guid id, IInventoryDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Inventories.ContainsKey(id))
            {
                Inventories[id] = item;
                return true;
            }
            return false;
        }
    }

    // Dummy implementation of IItemLogic
    internal class DummyItemLogic : IProductLogic
    {
        internal readonly Dictionary<Guid, IProductDataTransferObject> Items = new();

        public void Add(IProductDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Items[item.Id] = item;
        }

        public IEnumerable<IProductDataTransferObject> GetAll()
        {
            return Items.Values.ToList();
        }

        public IProductDataTransferObject? Get(Guid id)
        {
            Items.TryGetValue(id, out IProductDataTransferObject? item);
            return item;
        }

        public bool Remove(IProductDataTransferObject item)
        {
            if (item == null) return false;
            return Items.Remove(item.Id);
        }

        public bool RemoveById(Guid id)
        {
            return Items.Remove(id);
        }

        public bool Update(Guid id, IProductDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Items.ContainsKey(id))
            {
                Items[id] = item;
                return true;
            }
            return false;
        }
    }

    // Dummy implementation of IOrderLogic
    internal class DummyOrderLogic : IOrderLogic
    {
        internal readonly Dictionary<Guid, IOrderDataTransferObject> Orders = new();
        public int PeriodicProcessingCallCount { get; private set; } = 0;

        public void Add(IOrderDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Orders[item.Id] = item;
        }

        public IEnumerable<IOrderDataTransferObject> GetAll()
        {
            return Orders.Values.ToList();
        }

        public IOrderDataTransferObject? Get(Guid id)
        {
            Orders.TryGetValue(id, out IOrderDataTransferObject? order);
            return order;
        }

        public void PeriodicOrderProcessing()
        {
            PeriodicProcessingCallCount++;
        }

        public bool Remove(IOrderDataTransferObject item)
        {
            if (item == null) return false;
            return Orders.Remove(item.Id);
        }

        public bool RemoveById(Guid id)
        {
            return Orders.Remove(id);
        }

        public bool Update(Guid id, IOrderDataTransferObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Orders.ContainsKey(id))
            {
                Orders[id] = item;
                return true;
            }
            return false;
        }
    }


    internal class DummyCustomerDto : ICustomerDataTransferObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Money { get; set; }
        // Uses object reference as per the interface
        public IInventoryDataTransferObject Inventory { get; set; } = null!; // Initialize with null-forgiving, set in test setup
    }

    internal class DummyInventoryDto : IInventoryDataTransferObject
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        // Uses object references as per the interface
        public IEnumerable<IProductDataTransferObject> Items { get; set; } = Enumerable.Empty<IProductDataTransferObject>();
    }

    internal class DummyItemDto : IProductDataTransferObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int MaintenanceCost { get; set; }
    }

    internal class DummyOrderDto : IOrderDataTransferObject
    {
        public Guid Id { get; set; }
        // Uses object reference as per the interface
        public ICustomerDataTransferObject Buyer { get; set; } = null!; // Initialize with null-forgiving, set in test setup
        // Uses object references as per the interface
        public IEnumerable<IProductDataTransferObject> ItemsToBuy { get; set; } = Enumerable.Empty<IProductDataTransferObject>();
    }
}