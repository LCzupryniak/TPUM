using ClientServer.Shared.Logic.API;

namespace Client.Logic.Implementation
{
    internal class CustomerDataTransferObject : ICustomerDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public float Money { get; set; }
        public IInventoryDataTransferObject Inventory { get; set; }

        public CustomerDataTransferObject(Guid id, string name, float money, IInventoryDataTransferObject inventory)
        {
            Id = id;
            Name = name;
            Money = money;
            Inventory = inventory;
        }
    }
}
