using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Implementation
{
    internal class OrderModel : IOrderModel
    {
        public Guid Id { get; }
        public ICustomerModel Buyer { get; }
        public IEnumerable<IProductModel> ItemsToBuy { get; }

        // DTO
        public OrderModel(IOrderDataTransferObject dto)
        {
            Id = dto.Id;
            Buyer = new CustomerModel(dto.Buyer);
            ItemsToBuy = dto.ItemsToBuy?.Select(itemDto => new ProductModel(itemDto)).ToList() ?? Enumerable.Empty<IProductModel>();
        }

        // direct creation
        public OrderModel(Guid id, ICustomerModel buyer, IEnumerable<IProductModel> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy?.ToList() ?? new List<IProductModel>();
        }
    }
}
