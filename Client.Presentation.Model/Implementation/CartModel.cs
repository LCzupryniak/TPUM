using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CartModel : ICartModel
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductModel> Items { get; }

        // DTO
        public CartModel(ICartDataTransferObject dto)
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