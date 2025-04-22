using ClientServer.Shared.Data.API;
using ClientServer.Shared.Logic.API;

namespace Client.Logic.Implementation
{
    internal class MappedDataCustomer : ICustomer
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; private set; }
        public float Money { get; set; }
        public ICart Cart { get; private set; }

        public MappedDataCustomer(ICustomerDataTransferObject customerData)
        {
            Id = customerData.Id;
            Name = customerData.Name;
            Money = customerData.Money;
            Cart = new MappedDataCart(customerData.Cart);
        }
    }
}
