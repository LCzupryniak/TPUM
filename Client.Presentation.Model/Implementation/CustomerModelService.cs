using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CustomerModelService : ICustomerModelService
    {
        private readonly ICustomerLogic _customerLogic;
        private readonly ICartLogic _cartLogic;

        public CustomerModelService(ICustomerLogic customerLogic, ICartLogic cartLogic)
        {
            _customerLogic = customerLogic ?? throw new ArgumentNullException(nameof(customerLogic));
            _cartLogic = cartLogic ?? throw new ArgumentNullException(nameof(cartLogic));
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

        public void AddCustomer(Guid id, string name, float money, Guid cartId)
        {
            ICartDataTransferObject cartDto = _cartLogic.Get(cartId)!;
            TransientCustomerDTO? transientDto = new TransientCustomerDTO(id, name, money, cartDto);
            _customerLogic.Add(transientDto);
        }


        public bool RemoveCustomer(Guid id)
        {
            return _customerLogic.RemoveById(id);
        }

        public bool UpdateCustomer(Guid id, string name, float money, Guid cartId)
        {
            ICartDataTransferObject cartDto = _cartLogic.Get(cartId)!;

            TransientCustomerDTO? transientDto = new TransientCustomerDTO(id, name, money, cartDto);
            return _customerLogic.Update(id, transientDto);
        }

        public void TriggerPeriodicItemMaintenanceDeduction()
        {
            _customerLogic.PeriodicItemMaintenanceDeduction();
        }
    }
}
