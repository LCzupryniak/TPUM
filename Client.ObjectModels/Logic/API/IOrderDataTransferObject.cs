namespace Client.ObjectModels.Logic.API
{
    public interface IOrderDataTransferObject
    {
        public abstract Guid Id { get; }

        public abstract ICustomerDataTransferObject Buyer { get; }

        public abstract IEnumerable<IProductDataTransferObject> ItemsToBuy { get; }
    }
}
