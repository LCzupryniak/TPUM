namespace Server.Presentation
{
    [Serializable]
    public class SerializableProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int MaintenanceCost { get; set; }

        public SerializableProduct()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Price = 0;
            MaintenanceCost = 0;
        }

        public SerializableProduct(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
