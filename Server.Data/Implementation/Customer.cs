using ClientServer.Shared.Data.API;

namespace Server.Data.Implementation
{
    internal class Customer : ICustomer
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; private set; }
        public float Money { get; set; }
        public ICart Inventory { get; private set; }

        public Customer(string name, float money, ICart inventory)
        {
            Name = name;
            Money = money;
            Inventory = inventory;
        }

        public Customer(Guid id, string name, float money, ICart inventory)
        {
            Id = id;
            Name = name;
            Money = money;
            Inventory = inventory;
        }
    }
}
