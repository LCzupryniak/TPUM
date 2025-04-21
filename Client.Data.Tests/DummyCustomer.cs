using ClientServer.Shared.Data.API;

namespace Client.Data.Tests
{
    internal class DummyCustomer : ICustomer
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; private set; }
        public float Money { get; set; }
        public ICart Inventory { get; private set; }

        public DummyCustomer(string name, float money, ICart inventory)
        {
            Name = name;
            Money = money;
            Inventory = inventory;
        }

        public DummyCustomer(Guid id, string name, float money, ICart inventory)
        {
            Id = id;
            Name = name;
            Money = money;
            Inventory = inventory;
        }
    }
}
