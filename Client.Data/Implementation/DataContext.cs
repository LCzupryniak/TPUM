using Client.Data.Websocket;
using ClientServer.Shared.Data.API;
using ClientServer.Shared.WebSocket;
using Server.Presentation;
using System.Xml.Serialization;

namespace Client.Data.Implementation
{
    internal class DataContext : IDataContext
    {
        private readonly Dictionary<Guid, ICustomer> _customers = new Dictionary<Guid, ICustomer>();
        private readonly Dictionary<Guid, IProduct> _items = new Dictionary<Guid, IProduct>();
        private readonly Dictionary<Guid, ICart> _carts = new Dictionary<Guid, ICart>();
        private readonly Dictionary<Guid, IOrder> _orders = new Dictionary<Guid, IOrder>();

        public Dictionary<Guid, ICustomer> Customers => _customers;
        public Dictionary<Guid, IProduct> Items => _items;
        public Dictionary<Guid, ICart> Carts => _carts;
        public Dictionary<Guid, IOrder> Orders => _orders;

        public event Action OnDataChanged = delegate { };

        public DataContext()
        {
            WebSocketClient.OnMessage += WebSocketClient_OnMessage;
        }

        private void WebSocketClient_OnMessage(string obj)
        {
            Console.WriteLine($"[SERVER] Got message about {obj.Split('|')[0]}");

            switch (obj.Split('|')[0])
            {
                case "ITEMS":
                    SyncItems(obj.Split('|')[1]);
                    break;
                case "CUSTOMERS":
                    SyncCustomers(obj.Split('|')[1]);
                    break;
                case "CARTS":
                    SyncCarts(obj.Split('|')[1]);
                    break;
                default:
                    Console.WriteLine("Unknown message type");
                    return;
            }

            OnDataChanged.Invoke();
        }

        private void SyncItems(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableProduct>));
            using (StringReader reader = new StringReader(xml))
            {
                _items.Clear();

                List<SerializableProduct> items = (List<SerializableProduct>)serializer.Deserialize(reader)!;
                foreach (var item in items)
                {
                    _items[item.Id] = new Product(item.Id, item.Name, item.Price, item.MaintenanceCost);
                }
            }
        }

        private void SyncCustomers(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableCustomer>));
            using (StringReader reader = new StringReader(xml))
            {
                _customers.Clear();

                List<SerializableCustomer> customers = (List<SerializableCustomer>)serializer.Deserialize(reader)!;
                foreach (var customer in customers)
                {
                    List<IProduct> items = new List<IProduct>();
                    foreach (var item in customer.Cart.Items)
                    {
                        items.Add(new Product(item.Id, item.Name, item.Price, item.MaintenanceCost));
                    }

                    ICart inv = new Cart(customer.Cart.Id, customer.Cart.Capacity, items);
                    _customers[customer.Id] = new Customer(customer.Id, customer.Name, customer.Money, inv);
                    _carts[inv.Id] = inv;
                }
            }
        }

        private void SyncCarts(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableCart>));
            using (StringReader reader = new StringReader(xml))
            {
                _carts.Clear();

                List<SerializableCart> carts = (List<SerializableCart>)serializer.Deserialize(reader)!;
                foreach (var inv in carts)
                {
                    List<IProduct> items = new List<IProduct>();
                    foreach (var item in inv.Items)
                    {
                        items.Add(new Product(item.Id, item.Name, item.Price, item.MaintenanceCost));
                    }

                    _carts[inv.Id] = new Cart(inv.Id, inv.Capacity, items);
                }
            }
        }
    }
}