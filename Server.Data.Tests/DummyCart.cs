using ClientServer.Shared.Data.API;

namespace Server.Data.Tests
{
    internal class DummyCart : ICart
    {
        public Guid Id { get; } = Guid.Empty;
        public int Capacity { get; }
        public List<IProduct> Items { get; }

        public DummyCart(int capacity)
        {
            Capacity = capacity;
            Items = new List<IProduct>();
        }

        public DummyCart(Guid id, int capacity)
        {
            Id = id;
            Capacity = capacity;
            Items = new List<IProduct>();
        }
    }
}
