using ClientServer.Shared.Logic.API;
using ClientServer.Shared.WebSocket;
using Server.Logic.API;
using System.Globalization;
using System.Xml.Serialization;

namespace Server.Presentation
{
    internal static class Server
    {
        private static ICustomerLogic _customerLogic = LogicFactory.CreateCustomerLogic();
        private static ICartLogic _cartLogic = LogicFactory.CreateCartLogic();
        private static IProductLogic _itemLogic = LogicFactory.CreateItemLogic();
        private static IOrderLogic _orderLogic = LogicFactory.CreateOrderLogic();

        private static IMaintenanceTracker _maintenanceTracker = MaintenanceFactory.CreateTracker();
        private static IMaintenanceReporter _maintenanceReporter = MaintenanceFactory.CreateReporter();

        private static WebSocketConnection CurrentConnection = null!;

        private static Timer? _maintenanceTimer = null;
        private static TimeSpan _interval = TimeSpan.FromSeconds(5);

        private static async Task Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            _maintenanceReporter.Subscribe(_maintenanceTracker, () => { }, (Exception e) => { }, (ICustomerDataTransferObject next) =>
            {
                _customerLogic.DeduceMaintenanceCost(next);
            });

            Console.WriteLine($"Starting maintenance timer with interval {_interval.TotalSeconds}s.");
            _maintenanceTimer = new Timer(
                MaintenanceTick,
                null,
                _interval,
                _interval
            );

            Console.WriteLine("[Server]: Starting WebSocket server on port 9081...");
            await WebSocketServer.Server(9081, ConnectionHandler);
        }

        private static void ConnectionHandler(WebSocketConnection webSocketConnection)
        {
            CurrentConnection = webSocketConnection;
            webSocketConnection.onMessage = ParseMessage;
            webSocketConnection.onClose = () => { Console.WriteLine("[Server]: Connection closed"); };
            webSocketConnection.onError = () => { Console.WriteLine("[Server]: Connection error encountered"); };

            Console.WriteLine("[Server]: New connection established successfully");
        }

        private async static Task SyncItems()
        {
            IEnumerable<IProductDataTransferObject> items = _itemLogic.GetAll();
            List<SerializableProduct> itemsToSerialize = items.Select(item => new SerializableProduct(item.Id, item.Name, item.Price, item.MaintenanceCost)).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableProduct>));
            string xml;
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, itemsToSerialize);
                xml = writer.ToString();
            }

            // Send the xml to the client
            await CurrentConnection.SendAsync("ITEMS|" + xml);

            Console.WriteLine("[SERVER]: Sync items");
        }

        private async static Task SyncCarts()
        {
            IEnumerable<ICartDataTransferObject> inves = _cartLogic.GetAll();
            List<SerializableCart> cartsToSerialize = inves.Select(inv =>
                new SerializableCart(inv.Id, inv.Capacity,
                    inv.Items.Select(item => new SerializableProduct(item.Id, item.Name, item.Price, item.MaintenanceCost)).ToList())
                ).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableCart>));
            string xml;
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, cartsToSerialize);
                xml = writer.ToString();
            }

            // Send the xml to the client
            await CurrentConnection.SendAsync("CARTS|" + xml);

            Console.WriteLine("[SERVER]: Sync carts");
        }

        private async static Task SyncCustomers()
        {
            IEnumerable<ICustomerDataTransferObject> customers = _customerLogic.GetAll();
            List<SerializableCustomer> customersToSerialize = customers.Select(customer =>
                new SerializableCustomer(customer.Id, customer.Name, customer.Money,
                    new SerializableCart(customer.Cart.Id, customer.Cart.Capacity,
                    customer.Cart.Items.Select(item => new SerializableProduct(item.Id, item.Name, item.Price, item.MaintenanceCost)).ToList())
                )).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableCustomer>));
            string xml;
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, customersToSerialize);
                xml = writer.ToString();
            }

            // Send the xml to the client
            await CurrentConnection.SendAsync("CUSTOMERS|" + xml);

            Console.WriteLine("[SERVER]: Sync customers");
        }

        private static async Task CreateOrder(string message)
        {
            // Example message: "POST /orders/{id}/buyer/{buyerId}/items/{item1},{item2},..."
            string[] parts = message.Split(new[] { "POST /orders/", "/buyer/", "/items/" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
                return;

            Guid orderId = Guid.Parse(parts[0]);
            Guid buyerId = Guid.Parse(parts[1]);
            List<Guid> itemIds = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(Guid.Parse)
                                    .ToList();

            ICustomerDataTransferObject? buyerDto = _customerLogic.Get(buyerId);
            if (buyerDto == null)
                return;

            List<IProductDataTransferObject> itemDtos = new List<IProductDataTransferObject>();
            foreach (Guid itemId in itemIds)
            {
                IProductDataTransferObject? itemDto = _itemLogic.Get(itemId);
                if (itemDto == null)
                    return;

                itemDtos.Add(itemDto);
            }

            TransientOrderDTO transientDto = new TransientOrderDTO(orderId, buyerDto, itemDtos);
            _orderLogic.Add(transientDto);

            _orderLogic.PeriodicOrderProcessing(); // process the order immediately

            await SynchronizeWithClients();
        }

        private static async Task SynchronizeWithClients()
        {
            await SyncItems();
            await SyncCustomers();
            await SyncItems();
        }

        private static async void ParseMessage(string message)
        {
            Console.WriteLine($"[CLIENT]: {message}");

            if (message.Contains("GET /items"))
                await SyncItems();
            else if (message.Contains("GET /customers"))
                await SyncCustomers();
            else if (message.Contains("GET /carts"))
                await SyncCarts();
            else if (message.Contains("POST /orders"))
                await CreateOrder(message);
        }

        private static void MaintenanceTick(object? state)
        {
            foreach (ICustomerDataTransferObject customer in _customerLogic.GetAll())
            {
                _maintenanceTracker.Track(customer);
            }

            Task.Run(async () => { await SynchronizeWithClients(); });
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