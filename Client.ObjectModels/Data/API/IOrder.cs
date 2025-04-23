namespace Client.ObjectModels.Data.API
{
    public interface IOrder : IIdentifiable
    {
        public abstract ICustomer Buyer { get; }

        public abstract IEnumerable<IProduct> ItemsToBuy { get; }
    }
}
