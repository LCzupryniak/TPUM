using ClientServer.Shared.Logic.API;

namespace Client.Logic.Tests
{
    internal class DummyCartDataTransferObject : ICartDataTransferObject
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductDataTransferObject> Items { get; }

        public DummyCartDataTransferObject(Guid id, int capacity, IEnumerable<IProductDataTransferObject> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items;
        }
    }
}
