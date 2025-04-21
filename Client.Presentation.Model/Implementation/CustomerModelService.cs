using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CustomerModelService : ICustomerModelService
    {
        private readonly ICustomerLogic _customerLogic;
        private readonly ICartLogic _inventoryLogic;

        public CustomerModelService(ICustomerLogic customerLogic, ICartLogic inventoryLogic)
        {
            _customerLogic = customerLogic ?? throw new ArgumentNullException(nameof(customerLogic));
            _inventoryLogic = inventoryLogic ?? throw new ArgumentNullException(nameof(inventoryLogic));
        }

        public IEnumerable<ICustomerModel> GetAllCustomers()
        {
            return _customerLogic.GetAll()
                            .Select(dto => new CustomerModel(dto)); // Map DTO to Model
        }

        public ICustomerModel? GetCustomer(Guid id)
        {
            ICustomerDataTransferObject? dto = _customerLogic.Get(id);
            return dto == null ? null : new CustomerModel(dto); // Map DTO to Model
        }

        public void AddCustomer(Guid id, string name, float money, Guid inventoryId)
        {
            IInventoryDataTransferObject inventoryDto = _inventoryLogic.Get(inventoryId)!;
            TransientCustomerDTO? transientDto = new TransientCustomerDTO(id, name, money, inventoryDto);
            _customerLogic.Add(transientDto);
        }


        public bool RemoveCustomer(Guid id)
        {
            return _customerLogic.RemoveById(id);
        }

        public bool UpdateCustomer(Guid id, string name, float money, Guid inventoryId)
        {
            IInventoryDataTransferObject inventoryDto = _inventoryLogic.Get(inventoryId)!;

            TransientCustomerDTO? transientDto = new TransientCustomerDTO(id, name, money, inventoryDto);
            return _customerLogic.Update(id, transientDto);
        }

        public void TriggerPeriodicItemMaintenanceDeduction()
        {
            _customerLogic.PeriodicItemMaintenanceDeduction();
        }
    }
}
