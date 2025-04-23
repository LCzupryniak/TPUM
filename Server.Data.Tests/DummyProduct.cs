using Server.ObjectModels.Data.API;

namespace Server.Data.Tests
{
    internal class DummyProduct : IProduct
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public DummyProduct(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }

        public DummyProduct(string name, int price, int maintenanceCost)
        {
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
