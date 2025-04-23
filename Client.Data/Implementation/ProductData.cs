using Client.ObjectModels.Data.API;

namespace Client.Data.Implementation
{
    internal class ProductData : IProduct
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public ProductData(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }

        public ProductData(string name, int price, int maintenanceCost)
        {
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
