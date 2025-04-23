using Server.ObjectModels.Logic.API;

namespace Server.Logic.Tests
{
    internal class DummyCustomerDataTransferObject : ICustomerDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public float Money { get; set; }
        public ICartDataTransferObject Cart { get; set; }

        public DummyCustomerDataTransferObject(Guid id, string name, float money, ICartDataTransferObject cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }
}
