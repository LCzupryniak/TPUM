using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;

namespace Server.Logic.Implementation
{
    internal class MappedDataCustomer : ICustomer
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; private set; }
        public float Money { get; set; }
        public ICart Cart { get; private set; }

        public MappedDataCustomer(ICustomerDataTransferObject customerData)
        {
            Id = customerData.Id;
            Name = customerData.Name;
            Money = customerData.Money;
            Cart = new MappedDataCart(customerData.Cart);
        }
    }

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
                mappedItems.Add(new MappedDataItem(item));
            }

            Id = cartData.Id;
            Capacity = cartData.Capacity;
            Items = mappedItems;
        }
    }

    internal class MappedDataItem : IProduct
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public MappedDataItem(IProductDataTransferObject itemData)
        {
            Id = itemData.Id;
            Name = itemData.Name;
            Price = itemData.Price;
            MaintenanceCost = itemData.MaintenanceCost;
        }
    }

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
                mappedItems.Add(new MappedDataItem(item));
            }

            Id = orderData.Id;
            Buyer = new MappedDataCustomer(orderData.Buyer);
            ItemsToBuy = mappedItems;
        }
    }
}
