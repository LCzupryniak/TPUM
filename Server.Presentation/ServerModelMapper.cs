using Server.ObjectModels.Generated;
using Server.ObjectModels.Logic.API;

namespace Server.Presentation.Mapping
{
    internal static class ServerModelMapper
    {
        // --- Z DTO Logiki do XmlModels (do wysyłania) ---
        public static Item ToXmlModel(this IProductDataTransferObject dto)
        {
            if (dto == null) return null!;
            return new Item
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                MaintenanceCost = dto.MaintenanceCost
            };
        }

        public static Cart ToXmlModel(this ICartDataTransferObject dto)
        {
            if (dto == null) return null!;
            Cart xmlInv = new Cart { Id = dto.Id, Capacity = dto.Capacity };
            dto.Items?.ToList().ForEach(itemDto => xmlInv.Items.Add(itemDto.ToXmlModel()));
            return xmlInv;
        }

        public static Customer ToXmlModel(this ICustomerDataTransferObject dto)
        {
            if (dto == null) return null!;
            return new Customer
            {
                Id = dto.Id,
                Name = dto.Name,
                Money = dto.Money,
                Cart = dto.Cart?.ToXmlModel()
            };
        }

        public static Order ToXmlModel(this IOrderDataTransferObject dto)
        {
            if (dto == null) return null!;
            Order xmlOrder = new Order { Id = dto.Id, Buyer = dto.Buyer?.ToXmlModel() };
            // Mapuj rekursywnie listę przedmiotów do kupienia
            dto.ItemsToBuy?.ToList().ForEach(itemDto => xmlOrder.ItemsToBuy.Add(itemDto.ToXmlModel()));
            return xmlOrder;
        }

        // Helper do konwersji listy Item DTO na ArrayOfItem
        public static ArrayOfItem ToXmlModelArrayOfItem(this IEnumerable<IProductDataTransferObject> dtos)
        {
            ArrayOfItem xmlArray = new ArrayOfItem();
            if (dtos != null)
            {
                foreach (IProductDataTransferObject dto in dtos)
                {
                    Item xmlItem = dto.ToXmlModel();
                    if (xmlItem != null)
                    {
                        xmlArray.Item.Add(xmlItem);
                    }
                }
            }
            return xmlArray;
        }

        public static List<Customer> ToXmlModelList(this IEnumerable<ICustomerDataTransferObject> dtos)
        {
            if (dtos == null) return new List<Customer>();
            return dtos.Select(dto => dto.ToXmlModel()).Where(xml => xml != null).ToList()!;
        }

        public static List<Cart> ToXmlModelList(this IEnumerable<ICartDataTransferObject> dtos)
        {
            if (dtos == null) return new List<Cart>();
            return dtos.Select(dto => dto.ToXmlModel()).Where(xml => xml != null).ToList()!;
        }

        public static List<Order> ToXmlModelList(this IEnumerable<IOrderDataTransferObject> dtos)
        {
            if (dtos == null) return new List<Order>();
            return dtos.Select(dto => dto.ToXmlModel()).Where(xml => xml != null).ToList()!;
        }

        // --- Z XmlModels do DTO Logiki (do odbierania np. zamówień od klienta) ---
        public static IProductDataTransferObject ToLogicDto(this Item xml)
        {
            if (xml == null) return null!;
            return new TransientItemDTO(xml.Id, xml.Name, xml.Price, xml.MaintenanceCost);
        }

        public static ICartDataTransferObject ToLogicDto(this Cart xml)
        {
            if (xml == null) return null!;
            var items = xml.Items?.Select(i => i.ToLogicDto()).Where(i => i != null).ToList()
                        ?? new List<IProductDataTransferObject>();
            return new TransientCartDTO(xml.Id, xml.Capacity, items);
        }

        public static ICustomerDataTransferObject ToLogicDto(this Customer xml)
        {
            if (xml == null) return null!;
            ICartDataTransferObject cart = xml.Cart.ToLogicDto();
            return new TransientCustomerDTO(xml.Id, xml.Name, xml.Money, cart!);
        }

        public static IOrderDataTransferObject ToLogicDto(this Order xml)
        {
            if (xml == null) return null!;
            ICustomerDataTransferObject buyer = xml.Buyer.ToLogicDto();
            var items = xml.ItemsToBuy?.Select(i => i.ToLogicDto()).Where(i => i != null).ToList()
                        ?? new List<IProductDataTransferObject>();

            return new TransientOrderDTO(xml.Id, buyer!, items);
        }
    }
}
