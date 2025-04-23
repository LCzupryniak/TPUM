using Client.ObjectModels.Data.API;

namespace Client.Data.Implementation
{
    internal class OrderData : IOrder
    {
        public Guid Id { get; } = Guid.Empty;
        public ICustomer Buyer { get; }
        public IEnumerable<IProduct> ItemsToBuy { get; }

        public OrderData(ICustomer buyer, IEnumerable<IProduct> itemsToBuy)
        {
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }

        public OrderData(Guid id, ICustomer buyer, IEnumerable<IProduct> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }
    }
}
