namespace Client.Presentation.Model.API
{
    public interface IOrderModel
    {
        public abstract Guid Id { get; }
        public abstract ICustomerModel Buyer { get; }
        public abstract IEnumerable<IProductModel> ItemsToBuy { get; }
    }
}