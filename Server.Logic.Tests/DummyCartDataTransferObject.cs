using ClientServer.Shared.Logic.API;

namespace Server.Logic.Tests
{
    internal class DummyCartDataTransferObject : IInventoryDataTransferObject
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
