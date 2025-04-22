using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;

namespace Client.Logic.Implementation
{
    internal class MappedDataCart : ICart
    {
        public Guid Id { get; } = Guid.Empty;
        public int Capacity { get; }
        public List<IProduct> Items { get; }

        public MappedDataCart(ICartDataTransferObject cartData)
        {
            List<IProduct> mappedItems = new List<IProduct>();

            foreach (IProductDataTransferObject item in cartData.Items)
            {
                mappedItems.Add(new MappedDataProduct(item));
            }

            Id = cartData.Id;
            Capacity = cartData.Capacity;
            Items = mappedItems;
        }
    }
}
