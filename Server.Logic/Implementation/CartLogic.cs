using Server.ObjectModels.Data.API;
using Server.ObjectModels.Logic.API;

namespace Server.Logic.Implementation
{
    internal class CartLogic : ICartLogic
    {
        private IDataRepository _repository;
        private readonly object _lock = new object();

        public CartLogic(IDataRepository repository)
        {
            this._repository = repository;
        }

        public static ICartDataTransferObject Map(ICart cart)
        {
            List<IProductDataTransferObject> mappedItems = new List<IProductDataTransferObject>();

            foreach (IProduct item in cart.Items)
            {
                mappedItems.Add(ProductLogic.Map(item));
            }

            return new CartDataTransferObject(cart.Id, cart.Capacity, mappedItems);
        }

        public IEnumerable<ICartDataTransferObject> GetAll()
        {
            lock (_lock)
            {
                List<ICartDataTransferObject> all = new List<ICartDataTransferObject>();

                foreach (ICart cart in _repository.GetAllCarts())
                {
                    all.Add(Map(cart));
                }

                return all;
            }
        }

        public ICartDataTransferObject? Get(Guid id)
        {
            lock (_lock)
            {
                ICart? cart = _repository.GetCart(id);

                return cart is not null ? Map(cart) : null;
            }
        }

        public void Add(ICartDataTransferObject cart)
        {
            lock (_lock)
            {
                _repository.AddCart(new MappedDataCart(cart));
            }
        }

        public bool RemoveById(Guid id)
        {
            lock (_lock)
            {
                return _repository.RemoveCartById(id);
            }
        }

        public bool Remove(ICartDataTransferObject cart)
        {
            lock (_lock)
            {
                return _repository.RemoveCart(new MappedDataCart(cart));
            }
        }

        public bool Update(Guid id, ICartDataTransferObject cart)
        {
            lock (_lock)
            {
                return _repository.UpdateCart(id, new MappedDataCart(cart));
            }
        }
    }
}
