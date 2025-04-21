using ClientServer.Shared.Data.API;

namespace Server.Data.Implementation
{
    internal class Order : IOrder
    {
        public Guid Id { get; } = Guid.Empty;
        public ICustomer Buyer { get; }
        public IEnumerable<IProduct> ItemsToBuy { get; }

        public Order(ICustomer buyer, IEnumerable<IProduct> itemsToBuy)
        {
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }

        public Order(Guid id, ICustomer buyer, IEnumerable<IProduct> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }
    }
}
