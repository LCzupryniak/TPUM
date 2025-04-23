using Server.ObjectModels.Data.API;
using Server.ObjectModels.Logic.API;

namespace Server.Logic.Implementation
{
    internal class OrderLogic : IOrderLogic
    {
        private IDataRepository _repository;
        private readonly object _lock = new object();

        public OrderLogic(IDataRepository repository)
        {
            this._repository = repository;
        }

        public static IOrderDataTransferObject Map(IOrder order)
        {
            List<IProductDataTransferObject> mappedItems = new List<IProductDataTransferObject>();

            foreach (IProduct item in order.ItemsToBuy)
            {
                mappedItems.Add(ProductLogic.Map(item));
            }

            return new OrderDataTransferObject(order.Id, CustomerLogic.Map(order.Buyer), mappedItems);
        }

        public IEnumerable<IOrderDataTransferObject> GetAll()
        {
            lock (_lock)
            {
                List<IOrderDataTransferObject> all = new List<IOrderDataTransferObject>();

                foreach (IOrder order in _repository.GetAllOrders())
                {
                    all.Add(Map(order));
                }

                return all;
            }
        }

        public IOrderDataTransferObject? Get(Guid id)
        {
            lock (_lock)
            {
                IOrder? order = _repository.GetOrder(id);

                return order is not null ? Map(order) : null;
            }
        }

        public void Add(IOrderDataTransferObject order)
        {
            lock (_lock)
            {
                _repository.AddOrder(new MappedDataOrder(order));
            }
        }

        public bool RemoveById(Guid id)
        {
            lock (_lock)
            {
                return _repository.RemoveOrderById(id);
            }
        }

        public bool Remove(IOrderDataTransferObject order)
        {
            lock (_lock)
            {
                return _repository.RemoveOrder(new MappedDataOrder(order));
            }
        }

        public bool Update(Guid id, IOrderDataTransferObject order)
        {
            lock (_lock)
            {
                return _repository.UpdateOrder(id, new MappedDataOrder(order));
            }
        }

        public void PeriodicOrderProcessing()
        {
            lock (_lock)
            {
                foreach (IOrderDataTransferObject order in GetAll())
                {
                    ICustomerDataTransferObject buyer = order.Buyer;

                    List<IProductDataTransferObject> newCartItems = new List<IProductDataTransferObject>();
                    foreach (IProductDataTransferObject item in order.ItemsToBuy)
                    {
                        newCartItems.Add(item);

                        buyer.Money -= item.Price;

                        _repository.RemoveItem(new MappedDataItem(item));
                    }

                    foreach (IProductDataTransferObject item in buyer.Cart.Items)
                    {
                        newCartItems.Add(item);
                    }

                    buyer.Cart = new CartDataTransferObject(buyer.Cart.Id, buyer.Cart.Capacity, newCartItems);

                    _repository.UpdateCustomer(buyer.Id, new MappedDataCustomer(buyer));

                    _repository.RemoveOrder(new MappedDataOrder(order));
                }
            }
        }
    }
}
