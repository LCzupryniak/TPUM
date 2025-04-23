using Client.ObjectModels.Logic.API;

namespace Client.Logic.Tests
{
    internal class DummyProductDataTransferObject : IProductDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public DummyProductDataTransferObject(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
