using Client.ObjectModels.Data.API;

namespace Client.Data.Implementation
{
    internal class CustomerData : ICustomer
    {
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; private set; }
        public float Money { get; set; }
        public ICart Cart { get; private set; }

        public CustomerData(string name, float money, ICart cart)
        {
            Name = name;
            Money = money;
            Cart = cart;
        }

        public CustomerData(Guid id, string name, float money, ICart cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }
}
