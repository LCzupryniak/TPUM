using ClientServer.Shared.Logic.API;

namespace Client.Logic.Implementation
{
    internal class OrderDataTransferObject : IOrderDataTransferObject
    {
        public Guid Id { get; }
        public ICustomerDataTransferObject Buyer { get; }
        public IEnumerable<IProductDataTransferObject> ItemsToBuy { get; }

        public OrderDataTransferObject(Guid id, ICustomerDataTransferObject buyer, IEnumerable<IProductDataTransferObject> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }
    }
}
