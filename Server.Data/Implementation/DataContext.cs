using ClientServer.Shared.Data.API;

namespace Server.Data.Implementation
{
    internal class DataContext : IDataContext
    {
        private readonly Dictionary<Guid, ICustomer> _customers = new Dictionary<Guid, ICustomer>();
        private readonly Dictionary<Guid, IProduct> _items = new Dictionary<Guid, IProduct>();
        private readonly Dictionary<Guid, ICart> _inventories = new Dictionary<Guid, ICart>();
        private readonly Dictionary<Guid, IOrder> _orders = new Dictionary<Guid, IOrder>();

        public Dictionary<Guid, ICustomer> Customers => _customers;
        public Dictionary<Guid, IProduct> Items => _items;
        public Dictionary<Guid, ICart> Inventories => _inventories;
        public Dictionary<Guid, IOrder> Orders => _orders;

        public event Action OnDataChanged = delegate { };

        public DataContext()
        {
            Cart inv1 = new Cart(30);

            Guid customer1Guid = Guid.NewGuid();
            _customers.Add(customer1Guid, new Customer(customer1Guid, "Jan Nowak", 3000.0f, inv1));

            Guid item1Guid = Guid.NewGuid();
            _items.Add(item1Guid, new Product(item1Guid, "Hulajnoga elektryczna Pro 8.5 Lite ", 130, 35));

            Guid item2Guid = Guid.NewGuid();
            _items.Add(item2Guid, new Product(item2Guid, "Gogle VR Meta Oculus Quest 3", 150, 50));

            Guid item3Guid = Guid.NewGuid();
            _items.Add(item3Guid, new Product(item3Guid, "Projektor Samsung The Freestyler", 137, 20));

            Guid item4Guid = Guid.NewGuid();
            _items.Add(item4Guid, new Product(item4Guid, "Konsola Xbox Series S", 78, 10));

            Guid item5Guid = Guid.NewGuid();
            _items.Add(item5Guid, new Product(item5Guid, "Konsola Sony PlayStation 5", 130, 15));

            Guid item6Guid = Guid.NewGuid();
            Product item1 = new Product(item6Guid, "Głośnik JBL Partybox Encore", 200, 40);
            _items.Add(item6Guid, item1);

            inv1.Items.Add(item1);
        }
    }
}
