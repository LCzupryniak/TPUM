namespace Server.ObjectModels.WebSocket
{
    [Serializable]
    public class SerializableCustomer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Money { get; set; }

        public SerializableCart Cart { get; set; }

        public SerializableCustomer()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Money = 0;
            Cart = new SerializableCart();
        }

        public SerializableCustomer(Guid id, string name, float money, SerializableCart cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }
}
