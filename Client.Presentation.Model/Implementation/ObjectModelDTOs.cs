using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Implementation
{
    // Item DTO
    internal class TransientItemDTO : IProductDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public TransientItemDTO(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
        public TransientItemDTO(IProductModel model)
            : this(model.Id, model.Name, model.Price, model.MaintenanceCost) { }
    }

    // Cart DTO
    internal class TransientCartDTO : ICartDataTransferObject
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductDataTransferObject> Items { get; }

        public TransientCartDTO(Guid id, int capacity, IEnumerable<IProductDataTransferObject> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items?.ToList() ?? new List<IProductDataTransferObject>();
        }
        public TransientCartDTO(ICartModel model)
           : this(model.Id, model.Capacity, model.Items.Select(i => new TransientItemDTO(i))) { }
    }

    // Customer DTO
    internal class TransientCustomerDTO : ICustomerDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public float Money { get; set; }
        public ICartDataTransferObject Cart { get; set; }

        public TransientCustomerDTO(Guid id, string name, float money, ICartDataTransferObject cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
        public TransientCustomerDTO(ICustomerModel model)
             : this(model.Id, model.Name, model.Money, new TransientCartDTO(model.Cart)) { }
    }

    // Order DTO
    internal class TransientOrderDTO : IOrderDataTransferObject
    {
        public Guid Id { get; }
        public ICustomerDataTransferObject Buyer { get; }
        public IEnumerable<IProductDataTransferObject> ItemsToBuy { get; }

        public TransientOrderDTO(Guid id, ICustomerDataTransferObject buyer, IEnumerable<IProductDataTransferObject> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy?.ToList() ?? new List<IProductDataTransferObject>();
        }
        public TransientOrderDTO(IOrderModel model)
            : this(model.Id, new TransientCustomerDTO(model.Buyer), model.ItemsToBuy.Select(i => new TransientItemDTO(i))) { }
    }
}