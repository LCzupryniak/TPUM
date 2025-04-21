using ClientServer.Shared.Data.API;

namespace Client.Data.Implementation
{
    internal class Cart : ICart
    {
        public Guid Id { get; } = Guid.Empty;
        public int Capacity { get; }
        public List<IProduct> Items { get; }

        public Cart(int capacity)
        {
            Capacity = capacity;
            Items = new List<IProduct>();
        }

        public Cart(Guid id, int capacity)
        {
            Id = id;
            Capacity = capacity;
            Items = new List<IProduct>();
        }

        public Cart(Guid id, int capacity, List<IProduct> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items;
        }
    }
}
