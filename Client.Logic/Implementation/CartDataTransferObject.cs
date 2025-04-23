using Client.ObjectModels.Logic.API;

namespace Client.Logic.Implementation
{
    internal class CartDataTransferObject : ICartDataTransferObject
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductDataTransferObject> Items { get; }

        public CartDataTransferObject(Guid id, int capacity, IEnumerable<IProductDataTransferObject> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items;
        }
    }
}
