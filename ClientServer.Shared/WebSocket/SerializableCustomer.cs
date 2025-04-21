namespace ClientServer.Shared.WebSocket
{
    [Serializable]
    public class SerializableCustomer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Money { get; set; }

        public SerializableCart Inventory { get; set; }

        public SerializableCustomer()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Money = 0;
            Inventory = new SerializableCart();
        }

        public SerializableCustomer(Guid id, string name, float money, SerializableCart inventory)
        {
            Id = id;
            Name = name;
            Money = money;
            Inventory = inventory;
        }
    }
}
