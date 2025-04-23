using Server.ObjectModels.Logic.API;
using Server.ObjectModels.WebSocket;
using Server.ObjectModels.Generated;
using Server.Presentation.Mapping;
using Server.Logic.API;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;

namespace Server.Presentation
{
    public static class FileLogger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server_log.txt");
        private static readonly object FileLock = new object();

        public static void Log(string message)
        {
            try
            {
                lock (FileLock)
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | INFO | {message}{Environment.NewLine}";
                    File.AppendAllText(LogFilePath, logEntry);
                }
            }
            catch (Exception ex) { Console.WriteLine($"!!! LOG FAIL: {ex.Message} !!! {message}"); } // Awaryjny fallback
        }
        public static void LogError(string message, Exception? ex = null)
        {
            try
            {
                lock (FileLock)
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | ERROR| {message}{(ex != null ? $": {ex.Message}" : "")}{Environment.NewLine}";
                    if (ex?.StackTrace != null) logEntry += $" | StackTrace: {ex.StackTrace}{Environment.NewLine}";
                    File.AppendAllText(LogFilePath, logEntry);
                }
            }
            catch (Exception logEx) { Console.WriteLine($"!!! LOG FAIL: {logEx.Message} !!! {message} - {ex?.Message}"); }
        }
    }

    internal static class Server
    {
        private static ICustomerLogic _customerLogic = LogicFactory.CreateCustomerLogic();
        private static ICartLogic _cartLogic = LogicFactory.CreateCartLogic();
        private static IProductLogic _itemLogic = LogicFactory.CreateItemLogic();
        private static IOrderLogic _orderLogic = LogicFactory.CreateOrderLogic();

        private static IMaintenanceTracker _maintenanceTracker = MaintenanceFactory.CreateTracker();
        private static IMaintenanceReporter _maintenanceReporter = MaintenanceFactory.CreateReporter();

        private static WebSocketConnection? CurrentConnection = null!;

        private static readonly XmlSerializer ItemListSerializer = new XmlSerializer(typeof(ArrayOfItem));
        private static readonly XmlSerializer CustomerListSerializer = new XmlSerializer(typeof(List<Customer>), new XmlRootAttribute("ListOfCustomer"));
        private static readonly XmlSerializer CartListSerializer = new XmlSerializer(typeof(List<Cart>), new XmlRootAttribute("ListOfCart"));
        private static readonly XmlSerializer OrderSerializer = new XmlSerializer(typeof(Order));

        private static Timer? _maintenanceTimer = null;
        private static TimeSpan _interval = TimeSpan.FromSeconds(5);

        private static async Task Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            _maintenanceReporter.Subscribe(_maintenanceTracker,
               () => { FileLogger.Log("[MAINTENANCE] Tracker completed."); },
               (e) => { FileLogger.LogError("[MAINTENANCE] Tracker error", e); },
               (next) =>
               {
                   try { _customerLogic.DeduceMaintenanceCost(next); }
                   catch (Exception ex) { FileLogger.LogError($"[MAINTENANCE] Failed DeduceCost for Customer {next?.Id}", ex); }
               });

            _maintenanceTimer = new Timer(
                MaintenanceTick,
                null,
                _interval,
                _interval
            );

            //Console.WriteLine("[Server]: Starting WebSocket server on port 9081...");
            FileLogger.Log("[Server] Starting WebSocket server on port 9081...");
            await WebSocketServer.Server(9081, ConnectionHandler);
        }

        private static void ConnectionHandler(WebSocketConnection webSocketConnection)
        {
            CurrentConnection = webSocketConnection;
            webSocketConnection.onMessage = ParseMessage;
            webSocketConnection.onClose = () => { FileLogger.Log("[Server]: Connection closed"); };
            webSocketConnection.onError = () => { FileLogger.Log("[Server]: Connection error encountered"); };

            FileLogger.Log("[Server]: New connection established successfully");

        }

        private static async Task SendXmlAsync<T>(T data, XmlSerializer serializer) where T : class
        {
            if (CurrentConnection == null || data == null || serializer == null) return;
            try
            {
                string xml;
                using (StringWriter stringWriter = new StringWriter())
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false })) // Bez deklaracji, bez wcięć
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(xmlWriter, data, ns);
                    xml = stringWriter.ToString();
                }
                await CurrentConnection.SendAsync(xml);
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"[ERROR] Failed SendXmlAsync for type {typeof(T).Name}", ex);
            }
        }

        private async static Task SyncItems()
        {
            if (CurrentConnection == null) return;

            try
            {
                IEnumerable<IProductDataTransferObject> items = _itemLogic.GetAll();
                ArrayOfItem xmlData = items.ToXmlModelArrayOfItem();

                await SendXmlAsync(xmlData, ItemListSerializer);
                FileLogger.Log("[SERVER]: Sync items");
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"[ERROR] Failed to sync items: {ex.Message}");
            }
        }

        private async static Task SyncCarts()
        {
            if (CurrentConnection == null) return;
            try
            {
                IEnumerable<ICartDataTransferObject> inves = _cartLogic.GetAll();
                List<Cart> xmlData = inves.ToXmlModelList();

                await SendXmlAsync(xmlData, CartListSerializer);

                FileLogger.Log("[SERVER]: Sync carts");
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"[ERROR] Failed to sync carts: {ex.Message}");
            }

        }

        private async static Task SyncCustomers()
        {
            if (CurrentConnection == null) return;
            try
            {
                IEnumerable<ICustomerDataTransferObject> customers = _customerLogic.GetAll();
                List<Customer> xmlData = customers.ToXmlModelList();

                await SendXmlAsync(xmlData, CustomerListSerializer);

                FileLogger.Log("[SERVER]: Sync customers");
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"[ERROR] Failed to sync customers: {ex.Message}");
            }

        }

        private static async Task CreateOrder(IOrderDataTransferObject orderDto)
        {
            try
            {
                if (orderDto == null || orderDto.Buyer == null)
                {
                    FileLogger.LogError("[SERVER] Invalid order DTO received in CreateOrder.");
                    return;
                }

                ICustomerDataTransferObject customer = _customerLogic.Get(orderDto.Buyer.Id)!;

                List<IProductDataTransferObject> itemsToBuy = new List<IProductDataTransferObject>();

                foreach (IProductDataTransferObject item in orderDto.ItemsToBuy)
                {
                    itemsToBuy.Add(_itemLogic.Get(item.Id)!);
                }

                IOrderDataTransferObject order = new TransientOrderDTO(orderDto.Id, customer, itemsToBuy);

                _orderLogic.Add(order);
                _orderLogic.PeriodicOrderProcessing();

                FileLogger.Log($"[SERVER] Order {order.Id} added");
                await SynchronizeWithClients();
            }
            catch (Exception ex)
            {
                FileLogger.LogError($"[ERROR] Failed CreateOrder for ID {orderDto?.Id}", ex);
            }
        }

        private static async Task SynchronizeWithClients()
        {
            await SyncItems();
            await SyncCustomers();
            await SyncItems();
        }

        private static async void ParseMessage(string message)
        {
            FileLogger.Log($"[CLIENT]: {message}");

            if (message.Trim().StartsWith("<"))
            {
                // żądanie XML
                await HandleXmlRequest(message);
            }
            else
            {
                if (message.Contains("GET /items"))
                    await SyncItems();
                else if (message.Contains("GET /customers"))
                    await SyncCustomers();
                else if (message.Contains("GET /carts"))
                    await SyncCarts();
                //else if (message.Contains("POST /orders")) // powinno już lecieć po XML (HandleXmlRequest)
                //    await CreateOrder(message);
                else
                    Console.WriteLine($"[SERVER] Unknown command received: {message}");
            }

        }

        private static void MaintenanceTick(object? state)
        {
            foreach (ICustomerDataTransferObject customer in _customerLogic.GetAll())
            {
                _maintenanceTracker.Track(customer);
            }

            Task.Run(async () => { await SynchronizeWithClients(); });
        }

        private static async Task HandleXmlRequest(string xmlMessage)
        {
            string rootElementName;
            try
            {
                using (StringReader reader = new StringReader(xmlMessage))
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    xmlReader.MoveToContent();
                    rootElementName = xmlReader.Name;
                }
                FileLogger.Log($"[SERVER] Handling XML request with root: <{rootElementName}>");

                using (StringReader reader = new StringReader(xmlMessage))
                {
                    switch (rootElementName)
                    {
                        case "Order": // Klient wysyła zamówienie spakowane w XML
                            Order? xmlOrder = OrderSerializer.Deserialize(reader) as Order;
                            if (xmlOrder != null)
                            {
                                IOrderDataTransferObject? orderDto = xmlOrder.ToLogicDto();
                                if (orderDto != null)
                                {
                                    await CreateOrder(orderDto);
                                }
                                else { FileLogger.LogError("[SERVER] Failed mapping Order XML to DTO."); }
                            }
                            else { FileLogger.LogError("[SERVER] Failed deserializing Order XML."); }
                            break;

                        case "GetAllItemsRequest":
                            await SyncItems();
                            break;

                        case "GetAllCustomersRequest":
                            await SyncCustomers();
                            break;

                        case "GetAllCarts":
                            await SyncCarts();
                            break;

                        default:
                            FileLogger.Log($"[SERVER] No handler for XML request root: <{rootElementName}>.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to handle XML request: {ex.Message}");
                Console.WriteLine($"  Received XML: {xmlMessage.Substring(0, Math.Min(xmlMessage.Length, 500))}");
            }
        }
    }

    // Item DTO
    internal class TransientItemDTO : IProductDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        public TransientItemDTO(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }

    // Cart DTO
    internal class TransientCartDTO : ICartDataTransferObject
    {
        public Guid Id { get; }
        public int Capacity { get; }
        public IEnumerable<IProductDataTransferObject> Items { get; }

        public TransientCartDTO(Guid id, int capacity, IEnumerable<IProductDataTransferObject> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items?.ToList() ?? new List<IProductDataTransferObject>();
        }
    }

    // Customer DTO
    internal class TransientCustomerDTO : ICustomerDataTransferObject
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public float Money { get; set; }
        public ICartDataTransferObject Cart { get; set; }

        public TransientCustomerDTO(Guid id, string name, float money, ICartDataTransferObject cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }

    // Order DTO
    internal class TransientOrderDTO : IOrderDataTransferObject
    {
        public Guid Id { get; }
        public ICustomerDataTransferObject Buyer { get; }
        public IEnumerable<IProductDataTransferObject> ItemsToBuy { get; }

        public TransientOrderDTO(Guid id, ICustomerDataTransferObject buyer, IEnumerable<IProductDataTransferObject> itemsToBuy)
        {
            Id = id;
            Buyer = buyer;
            ItemsToBuy = itemsToBuy?.ToList() ?? new List<IProductDataTransferObject>();
        }
    }
}