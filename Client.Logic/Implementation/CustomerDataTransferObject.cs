using Client.ObjectModels.Logic.API;

namespace Client.Logic.Implementation
{
    internal class CustomerDataTransferObject : ICustomerDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public float Money { get; set; }
        public ICartDataTransferObject Cart { get; set; }

        public CustomerDataTransferObject(Guid id, string name, float money, ICartDataTransferObject cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }
}
