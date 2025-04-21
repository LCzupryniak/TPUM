using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Implementation
{
    internal class ProductModelService : IProductModelService
    {
        private readonly IProductLogic _itemLogic;

        // Inject Logic API
        public ProductModelService(IProductLogic itemLogic)
        {
            _itemLogic = itemLogic ?? throw new ArgumentNullException(nameof(itemLogic));
        }

        public IEnumerable<IProductModel> GetAllItems()
        {
            return _itemLogic.GetAll()
                             .Select(dto => new ProductModel(dto)); // Map DTO to Model
        }

        public IProductModel? GetItem(Guid id)
        {
            IProductDataTransferObject? dto = _itemLogic.Get(id);
            return dto == null ? null : new ProductModel(dto); // Map DTO to Model
        }

        public void AddItem(Guid id, string name, int price, int maintenanceCost)
        {
            // Create Model DTO to pass to the logic layer
            TransientItemDTO modelDto = new TransientItemDTO(id, name, price, maintenanceCost);
            _itemLogic.Add(modelDto);
        }

        public bool RemoveItem(Guid id)
        {
            return _itemLogic.RemoveById(id);
        }

        public bool UpdateItem(Guid id, string name, int price, int maintenanceCost)
        {
            // Create a Model DTO to pass to the logic layer
            TransientItemDTO modelDto = new TransientItemDTO(id, name, price, maintenanceCost);
            return _itemLogic.Update(id, modelDto);
        }
    }
}
