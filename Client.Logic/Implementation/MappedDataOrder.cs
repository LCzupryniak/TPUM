using Client.ObjectModels.Data.API;
using Client.ObjectModels.Logic.API;

namespace Client.Logic.Implementation
{
    internal class MappedDataOrder : IOrder
    {
        public Guid Id { get; } = Guid.Empty;
        public ICustomer Buyer { get; }
        public IEnumerable<IProduct> ItemsToBuy { get; }

        public MappedDataOrder(IOrderDataTransferObject orderData)
        {
            List<IProduct> mappedItems = new List<IProduct>();

            foreach (IProductDataTransferObject item in orderData.ItemsToBuy)
            {
                mappedItems.Add(new MappedDataProduct(item));
            }

            Id = orderData.Id;
            Buyer = new MappedDataCustomer(orderData.Buyer);
            ItemsToBuy = mappedItems;
        }
    }
}
