using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CartModel : ICartModel
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductModel> Items { get; }

        // DTO
        public CartModel(IInventoryDataTransferObject dto)
        {
            Id = dto.Id;
            Capacity = dto.Capacity;
            Items = dto.Items?.Select(itemDto => new ProductModel(itemDto)).ToList() ?? Enumerable.Empty<IProductModel>();
        }

        // direct creation
        public CartModel(Guid id, int capacity, IEnumerable<IProductModel> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items ?? Enumerable.Empty<IProductModel>();
        }
    }
}