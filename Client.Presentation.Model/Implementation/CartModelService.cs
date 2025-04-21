using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CartModelService : ICartModelService
    {
        private readonly ICartLogic _inventoryLogic;

        public CartModelService(ICartLogic inventoryLogic)
        {
            _inventoryLogic = inventoryLogic ?? throw new ArgumentNullException(nameof(inventoryLogic));
        }

        public void Add(Guid id, int capacity)
        {
            _inventoryLogic.Add(new TransientInventoryDTO(id, capacity, new List<IProductDataTransferObject>()));
        }

        public IEnumerable<ICartModel> GetAllInventories()
        {
            return _inventoryLogic.GetAll()
                                 .Select(dto => new CartModel(dto)); // Map DTO to Model
        }

        public ICartModel? GetInventory(Guid id)
        {
            IInventoryDataTransferObject? dto = _inventoryLogic.Get(id);
            return dto == null ? null : new CartModel(dto); // Map DTO to Model
        }
    }
}