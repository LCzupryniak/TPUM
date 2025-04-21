namespace ClientServer.Shared.Data.API
{
    public interface IDataRepository
    {
        public abstract IEnumerable<ICustomer> GetAllCustomers();
        public abstract ICustomer? GetCustomer(Guid id);
        public abstract void AddCustomer(ICustomer customer);
        public abstract bool RemoveCustomerById(Guid id);
        public abstract bool RemoveCustomer(ICustomer customer);
        public abstract bool UpdateCustomer(Guid id, ICustomer customer);

        public abstract IEnumerable<ICart> GetAllInventories();
        public abstract ICart? GetInventory(Guid id);
        public abstract void AddInventory(ICart inventory);
        public abstract bool RemoveInventoryById(Guid id);
        public abstract bool RemoveInventory(ICart inventory);
        public abstract bool UpdateInventory(Guid id, ICart inventory);

        public abstract IEnumerable<IProduct> GetAllItems();
        public abstract IProduct? GetItem(Guid id);
        public abstract void AddItem(IProduct item);
        public abstract bool RemoveItemById(Guid id);
        public abstract bool RemoveItem(IProduct item);
        public abstract bool UpdateItem(Guid id, IProduct item);

        public abstract IEnumerable<IOrder> GetAllOrders();
        public abstract IOrder? GetOrder(Guid id);
        public abstract void AddOrder(IOrder order);
        public abstract bool RemoveOrderById(Guid id);
        public abstract bool RemoveOrder(IOrder order);
        public abstract bool UpdateOrder(Guid id, IOrder order);

        public abstract event Action OnDataChanged;
    }
}
