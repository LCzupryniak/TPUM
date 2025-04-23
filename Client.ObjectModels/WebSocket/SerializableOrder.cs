namespace Client.ObjectModels.WebSocket
{
    public class SerializableOrder
    {
        public Guid Id { get; set; }
        public SerializableCustomer Buyer { get; set; }
        public List<SerializableProduct> ItemsToBuy { get; set; }

        public SerializableOrder()
        {
            Id = Guid.NewGuid();
            Buyer = new SerializableCustomer();
            ItemsToBuy = new List<SerializableProduct>();
        }

        public SerializableOrder(Guid id, SerializableCustomer buyer, List<SerializableProduct> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy;
        }
    }
}
