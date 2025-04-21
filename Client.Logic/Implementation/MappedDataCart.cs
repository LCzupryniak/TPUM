using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;

namespace Client.Logic.Implementation
{
    internal class MappedDataCart : ICart
    {
        public Guid Id { get; } = Guid.Empty;
        public int Capacity { get; }
        public List<IProduct> Items { get; }

        public MappedDataCart(IInventoryDataTransferObject inventoryData)
        {
            List<IProduct> mappedItems = new List<IProduct>();

            foreach (IProductDataTransferObject item in inventoryData.Items)
            {
                mappedItems.Add(new MappedDataProduct(item));
            }

            Id = inventoryData.Id;
            Capacity = inventoryData.Capacity;
            Items = mappedItems;
        }
    }
}
