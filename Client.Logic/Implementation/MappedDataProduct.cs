using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;

namespace Client.Logic.Implementation
{
    internal class MappedDataProduct : IProduct
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public MappedDataProduct(IProductDataTransferObject itemData)
        {
            Id = itemData.Id;
            Name = itemData.Name;
            Price = itemData.Price;
            MaintenanceCost = itemData.MaintenanceCost;
        }
    }
}
