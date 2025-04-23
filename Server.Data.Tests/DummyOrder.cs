using Server.ObjectModels.Data.API;

namespace Server.Data.Tests
{
    internal class DummyOrder : IOrder
    {
        public Guid Id { get; } = Guid.Empty;
        public ICustomer Buyer { get; }
        public IEnumerable<IProduct> ItemsToBuy { get; }

        public DummyOrder(ICustomer buyer, IEnumerable<IProduct> itemsToBuy)
        {
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }

        public DummyOrder(Guid id, ICustomer buyer, IEnumerable<IProduct> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }
    }
}
