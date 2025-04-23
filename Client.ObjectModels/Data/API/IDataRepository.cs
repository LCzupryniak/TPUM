namespace Client.ObjectModels.Data.API
{
    public interface IDataRepository
    {
        public abstract IEnumerable<ICustomer> GetAllCustomers();
        public abstract ICustomer? GetCustomer(Guid id);
        public abstract void AddCustomer(ICustomer customer);
        public abstract bool RemoveCustomerById(Guid id);
        public abstract bool RemoveCustomer(ICustomer customer);
        public abstract bool UpdateCustomer(Guid id, ICustomer customer);

        public abstract IEnumerable<ICart> GetAllCarts();
        public abstract ICart? GetCart(Guid id);
        public abstract void AddCart(ICart cart);
        public abstract bool RemoveCartById(Guid id);
        public abstract bool RemoveCart(ICart cart);
        public abstract bool UpdateCart(Guid id, ICart cart);

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
