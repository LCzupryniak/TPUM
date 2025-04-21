using ClientServer.Shared.Logic.API;

namespace Server.Logic.Tests
{
    internal class DummyCustomerDataTransferObject : ICustomerDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public float Money { get; set; }
        public IInventoryDataTransferObject Inventory { get; set; }

        public DummyCustomerDataTransferObject(Guid id, string name, float money, IInventoryDataTransferObject inventory)
        {
            Id = id;
            Name = name;
            Money = money;
            Inventory = inventory;
        }
    }
}
