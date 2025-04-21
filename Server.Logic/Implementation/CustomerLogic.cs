using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;

namespace Server.Logic.Implementation
{
    internal class CustomerLogic : ICustomerLogic
    {
        private IDataRepository _repository;
        private readonly object _lock = new object();

        public CustomerLogic(IDataRepository repository)
        {
            this._repository = repository;
        }

        public static ICustomerDataTransferObject Map(ICustomer customer)
        {
            return new CustomerDataTransferObject(customer.Id, customer.Name, customer.Money, CartLogic.Map(customer.Inventory));
        }

        public IEnumerable<ICustomerDataTransferObject> GetAll()
        {
            lock (_lock)
            {
                List<ICustomerDataTransferObject> all = new List<ICustomerDataTransferObject>();

                foreach (ICustomer customer in _repository.GetAllCustomers())
                {
                    all.Add(Map(customer));
                }

                return all;
            }
        }

        public ICustomerDataTransferObject? Get(Guid id)
        {
            lock (_lock)
            {
                ICustomer? customer = _repository.GetCustomer(id);

                return customer is not null ? Map(customer) : null;
            }
        }

        public void Add(ICustomerDataTransferObject customer)
        {
            lock (_lock)
            {
                _repository.AddCustomer(new MappedDataCustomer(customer));
            }
        }

        public bool RemoveById(Guid id)
        {
            lock (_lock)
            {
                return _repository.RemoveCustomerById(id);
            }
        }

        public bool Remove(ICustomerDataTransferObject customer)
        {
            lock (_lock)
            {
                return _repository.RemoveCustomer(new MappedDataCustomer(customer));
            }
        }

        public bool Update(Guid id, ICustomerDataTransferObject customer)
        {
            lock (_lock)
            {
                return _repository.UpdateCustomer(id, new MappedDataCustomer(customer));
            }
        }

        public void PeriodicItemMaintenanceDeduction()
        {
            lock (_lock)
            {
                foreach (ICustomerDataTransferObject customer in GetAll())
                {
                    foreach (IProductDataTransferObject item in customer.Inventory.Items)
                    {
                        customer.Money -= item.MaintenanceCost;
                    }

                    _repository.UpdateCustomer(customer.Id, new MappedDataCustomer(customer));
                }
            }
        }

        public void DeduceMaintenanceCost(ICustomerDataTransferObject customer)
        {
            lock (_lock)
            {
                foreach (IProductDataTransferObject item in customer.Inventory.Items)
                {
                    customer.Money -= item.MaintenanceCost;
                }

                _repository.UpdateCustomer(customer.Id, new MappedDataCustomer(customer));
            }
        }
    }
}
