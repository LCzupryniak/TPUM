using Client.ObjectModels.Data.API;
using Client.ObjectModels.Generated;


namespace Client.Data.Mapping
{
    internal static class ModelMapper
    {
        // --- Mapowanie z XmlModels do Modeli Wewnętrznych ---

        public static IProduct ToInternalModel(this Item xmlItem)
        {
            if (xmlItem == null) return null!;
            return new Implementation.ProductData(xmlItem.Id, xmlItem.Name, xmlItem.Price, xmlItem.MaintenanceCost);
        }

        public static ICart ToInternalModel(this Cart xmlCart)
        {
            if (xmlCart == null) return null!;

            List<IProduct> internalItems = xmlCart.Items?
                .Select(xmlItem => xmlItem.ToInternalModel())
                .Where(item => item != null)
                .ToList() ?? new List<IProduct>();

            return new Implementation.CartData(xmlCart.Id, xmlCart.Capacity, internalItems);
        }

        public static ICustomer ToInternalModel(this Customer xmlCustomer)
        {
            if (xmlCustomer == null) return null!;

            ICart? internalCart = xmlCustomer.Cart?.ToInternalModel();

            return new Implementation.CustomerData(xmlCustomer.Id, xmlCustomer.Name, xmlCustomer.Money, internalCart!);
        }

        public static IOrder ToInternalModel(this Order xmlOrder, Dictionary<Guid, ICustomer> existingCustomers, Dictionary<Guid, IProduct> existingItems)
        {
            if (xmlOrder == null) return null!;

            ICustomer? buyer = null;
            if (xmlOrder.Buyer != null && !existingCustomers.TryGetValue(xmlOrder.Buyer.Id, out buyer))
            {
                // buyerless order
                Console.WriteLine($"Buyer with ID {xmlOrder.Buyer.Id} not found for Order {xmlOrder.Id}.");
                 return null;
            }

            List<IProduct> itemsToBuy = new List<IProduct>();
            if (xmlOrder.ItemsToBuy != null)
            {
                foreach (Item xmlItem in xmlOrder.ItemsToBuy)
                {
                    if (xmlItem != null && existingItems.TryGetValue(xmlItem.Id, out var internalItem))
                    {
                        itemsToBuy.Add(internalItem);
                    }
                    else if (xmlItem != null)
                    {
                        Console.WriteLine($"Item with ID {xmlItem.Id} not found for Order {xmlOrder.Id}.");
                    }
                }
            }
            return new Implementation.OrderData(xmlOrder.Id, buyer!, itemsToBuy);
        }


        // z Modeli Wewnętrznych do XmlModels
        public static Item ToXmlModel(this IProduct internalItem)
        {
            if (internalItem == null) return null!;
            return new Item
            {
                Id = internalItem.Id,
                Name = internalItem.Name,
                Price = internalItem.Price,
                MaintenanceCost = internalItem.MaintenanceCost
            };
        }

        public static Cart ToXmlModel(this ICart internalCart)
        {
            if (internalCart == null) return null!;
            Cart xmlCart = new Cart
            {
                Id = internalCart.Id,
                Capacity = internalCart.Capacity
            };
            internalCart.Items?.ForEach(item => xmlCart.Items.Add(item.ToXmlModel()));
            return xmlCart;
        }

        public static Customer ToXmlModel(this ICustomer internalCustomer)
        {
            if (internalCustomer == null) return null!;
            return new Customer
            {
                Id = internalCustomer.Id,
                Name = internalCustomer.Name,
                Money = internalCustomer.Money,
                Cart = internalCustomer.Cart?.ToXmlModel()
            };
        }

        public static Order ToXmlModel(this IOrder internalOrder)
        {
            if (internalOrder == null) return null!;
            Order xmlOrder = new Order
            {
                Id = internalOrder.Id,
                Buyer = internalOrder.Buyer?.ToXmlModel()
            };
            internalOrder.ItemsToBuy?.ToList().ForEach(item => xmlOrder.ItemsToBuy.Add(item.ToXmlModel()));
            return xmlOrder;
        }
    }
}