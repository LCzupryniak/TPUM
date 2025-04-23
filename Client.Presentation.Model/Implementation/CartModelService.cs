using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CartModelService : ICartModelService
    {
        private readonly ICartLogic _cartLogic;

        public CartModelService(ICartLogic cartLogic)
        {
            _cartLogic = cartLogic ?? throw new ArgumentNullException(nameof(cartLogic));
        }

        public void Add(Guid id, int capacity)
        {
            _cartLogic.Add(new TransientCartDTO(id, capacity, new List<IProductDataTransferObject>()));
        }

        public IEnumerable<ICartModel> GetAllCarts()
        {
            return _cartLogic.GetAll()
                                 .Select(dto => new CartModel(dto)); // Map DTO to Model
        }

        public ICartModel? GetCart(Guid id)
        {
            ICartDataTransferObject? dto = _cartLogic.Get(id);
            return dto == null ? null : new CartModel(dto); // Map DTO to Model
        }
    }
}