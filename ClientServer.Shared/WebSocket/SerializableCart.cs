using Server.Presentation;

namespace ClientServer.Shared.WebSocket
{
    [Serializable]
    public class SerializableCart
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public List<SerializableProduct> Items { get; set; }

        public SerializableCart()
        {
            Id = Guid.NewGuid();
            Capacity = 0;
            Items = new List<SerializableProduct>();
        }

        public SerializableCart(Guid id, int capacity, List<SerializableProduct> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items;
        }
    }
}
