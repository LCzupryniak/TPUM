using Client.Data.Websocket;
using System.Xml.Serialization;
using Client.Data.Mapping;
using Client.ObjectModels.Generated;
using Client.ObjectModels.Data.API;
using System.Xml;
using System.Diagnostics;

namespace Client.Data.Implementation
{
    internal class DataContext : IDataContext
    {
        private readonly Dictionary<Guid, ICustomer> _customers = new Dictionary<Guid, ICustomer>();
        private readonly Dictionary<Guid, IProduct> _items = new Dictionary<Guid, IProduct>();
        private readonly Dictionary<Guid, ICart> _carts = new Dictionary<Guid, ICart>();
        private readonly Dictionary<Guid, IOrder> _orders = new Dictionary<Guid, IOrder>();

        private static readonly XmlSerializer ItemListSerializer = new XmlSerializer(typeof(ArrayOfItem));
        private static readonly XmlSerializer CustomerListSerializer = new XmlSerializer(typeof(List<Customer>), new XmlRootAttribute("ListOfCustomer"));
        private static readonly XmlSerializer CartListSerializer = new XmlSerializer(typeof(List<Cart>), new XmlRootAttribute("ListOfCart"));
        private static readonly XmlSerializer OrderListSerializer = new XmlSerializer(typeof(List<Order>), new XmlRootAttribute("ListOfOrder"));

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

            if (string.IsNullOrWhiteSpace(obj)) return;

            Debug.WriteLine($"[CLIENT] Received XML {obj.Split('|')[0]}");


            string rootElementName;

            try
            {
                using (StringReader reader = new StringReader(obj))
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    xmlReader.MoveToContent();
                    rootElementName = xmlReader.Name;
                }

                Debug.WriteLine($"[CLIENT] Root element: <{rootElementName}>");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CLIENT] Failed to read root element from XML: {ex.Message}. XML: {obj}");
                return;
            }

            try
            {
                bool dataChanged = false;
                using (StringReader reader = new StringReader(obj))
                {
                    switch (rootElementName)
                    {
                        case "ArrayOfItem":
                            ArrayOfItem? itemsWrapper = ItemListSerializer.Deserialize(reader) as ArrayOfItem;
                            if (itemsWrapper?.Item != null)
                            {
                                SyncItems(itemsWrapper.Item);
                                dataChanged = true;
                            }
                            break;

                        case "ListOfCustomer":
                            List<Customer>? customers = CustomerListSerializer.Deserialize(reader) as List<Customer>;
                            if (customers != null)
                            {
                                SyncCustomers(customers);
                                dataChanged = true;
                            }
                            break;

                        case "ListOfCart":
                            List<Cart>? carts = CartListSerializer.Deserialize(reader) as List<Cart>;
                            if (carts != null)
                            {
                                SyncCarts(carts);
                                dataChanged = true;
                            }
                            break;

                        default:
                            Debug.WriteLine($"[CLIENT] No handler for root: <{rootElementName}>");
                            break;
                    }
                }

                if (dataChanged)
                    OnDataChanged.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CLIENT] Deserialization Error: {ex.Message} - for '<{rootElementName}>': ");
                Debug.WriteLine($"  Received XML Snippet: {obj.Substring(0, Math.Min(obj.Length, 500))}");
            }
        }

        private void SyncItems(ICollection<Item> xmlItems)
        {
            lock (_items)
            {
                _items.Clear();
                foreach (Item xmlItem in xmlItems.Where(x => x != null))
                {
                    IProduct internalItem = xmlItem.ToInternalModel(); // --- Mapper ---
                    if (internalItem != null)
                    {
                        _items[internalItem.Id] = internalItem;
                    }
                }
            }

        }

        private void SyncCustomers(List<Customer> xmlCustomers)
        {
            lock (_customers)
                lock (_carts)
                {
                    _carts.Clear();
                    _customers.Clear();
                    foreach (Customer xmlCustomer in xmlCustomers.Where(x => x != null))
                    {
                        ICustomer internalCustomer = xmlCustomer.ToInternalModel();
                        if (internalCustomer != null)
                        {
                            _customers[internalCustomer.Id] = internalCustomer;

                            if (internalCustomer.Cart != null && !_carts.ContainsKey(internalCustomer.Cart.Id))
                            {
                                _carts[internalCustomer.Cart.Id] = internalCustomer.Cart;
                            }
                        }
                    }
                }
        }

        private void SyncCarts(List<Cart> xmlCarts)
        {
            lock (_carts)
            {
                _carts.Clear();
                foreach (Cart xmlInv in xmlCarts.Where(x => x != null))
                {
                    ICart internalInv = xmlInv.ToInternalModel();
                    if (internalInv != null)
                    {
                        _carts[internalInv.Id] = internalInv;
                    }
                }
            }
        }
    }
}