using ClientServer.Shared.Data.API;

namespace Server.Data.Implementation
{
    internal class Product : IProduct
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public Product(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }

        public Product(string name, int price, int maintenanceCost)
        {
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
