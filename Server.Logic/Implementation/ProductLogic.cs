﻿using Server.ObjectModels.Data.API;
using Server.ObjectModels.Logic.API;

namespace Server.Logic.Implementation
{
    internal class ProductLogic : IProductLogic
    {
        private IDataRepository _repository;
        private readonly object _lock = new object();

        public ProductLogic(IDataRepository repository)
        {
            this._repository = repository;
        }

        public static IProductDataTransferObject Map(IProduct item)
        {
            return new ItemDataTransferObject(item.Id, item.Name, item.Price, item.MaintenanceCost);
        }

        public IEnumerable<IProductDataTransferObject> GetAll()
        {
            lock (_lock)
            {
                List<IProductDataTransferObject> all = new List<IProductDataTransferObject>();

                foreach (IProduct item in _repository.GetAllItems())
                {
                    all.Add(Map(item));
                }

                return all;
            }
        }

        public IProductDataTransferObject? Get(Guid id)
        {
            lock (_lock)
            {
                IProduct? item = _repository.GetItem(id);

                return item is not null ? Map(item) : null;
            }
        }

        public void Add(IProductDataTransferObject item)
        {
            lock (_lock)
            {
                _repository.AddItem(new MappedDataItem(item));
            }
        }

        public bool RemoveById(Guid id)
        {
            lock (_lock)
            {
                return _repository.RemoveItemById(id);
            }
        }

        public bool Remove(IProductDataTransferObject item)
        {
            lock (_lock)
            {
                return _repository.RemoveItem(new MappedDataItem(item));
            }
        }

        public bool Update(Guid id, IProductDataTransferObject item)
        {
            lock (_lock)
            {
                return _repository.UpdateItem(id, new MappedDataItem(item));
            }
        }
    }
}
