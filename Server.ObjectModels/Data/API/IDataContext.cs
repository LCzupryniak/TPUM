namespace Server.ObjectModels.Data.API
{
    public interface IDataContext
    {
        public abstract Dictionary<Guid, ICustomer> Customers { get; }

        public abstract Dictionary<Guid, IProduct> Items { get; }

        public abstract Dictionary<Guid, ICart> Carts { get; }

        public abstract Dictionary<Guid, IOrder> Orders { get; }

        public abstract event Action OnDataChanged;
    }
}
