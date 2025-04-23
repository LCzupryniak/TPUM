using Client.ObjectModels.Data.API;

namespace Client.Data.Implementation
{
    internal class CartData : ICart
    {
        public Guid Id { get; } = Guid.Empty;
        public int Capacity { get; }
        public List<IProduct> Items { get; }

        public CartData(int capacity)
        {
            Capacity = capacity;
            Items = new List<IProduct>();
        }

        public CartData(Guid id, int capacity)
        {
            Id = id;
            Capacity = capacity;
            Items = new List<IProduct>();
        }

        public CartData(Guid id, int capacity, List<IProduct> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items;
        }
    }
}
