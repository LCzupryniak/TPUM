using Server.ObjectModels.Data.API;

namespace Server.Data.Implementation
{
    internal class Customer : ICustomer
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; private set; }
        public float Money { get; set; }
        public ICart Cart { get; private set; }

        public Customer(string name, float money, ICart cart)
        {
            Name = name;
            Money = money;
            Cart = cart;
        }

        public Customer(Guid id, string name, float money, ICart cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }
}
