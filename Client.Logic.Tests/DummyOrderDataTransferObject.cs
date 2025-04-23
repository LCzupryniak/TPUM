using Client.ObjectModels.Logic.API;

namespace Client.Logic.Tests
{
    internal class DummyOrderDataTransferObject : IOrderDataTransferObject
    {
        public Guid Id { get; }
        public ICustomerDataTransferObject Buyer { get; }
        public IEnumerable<IProductDataTransferObject> ItemsToBuy { get; }

        public DummyOrderDataTransferObject(Guid id, ICustomerDataTransferObject buyer, IEnumerable<IProductDataTransferObject> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }
    }
}
