using Client.ObjectModels.Logic.API;

namespace Client.Logic.Implementation
{
    internal class ProductDataTransferObject : IProductDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public ProductDataTransferObject(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
