using Client.Data.Websocket;
using Client.ObjectModels.Generated;
using Client.Logic.API;
using System.Net.WebSockets;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Client.Logic.Implementation
{
    public class ClientConnectionService : IConnectionService
    {
        public Action<string> ConnectionLogger { get => _logger; set => _logger = value; }

        public Action? onDataArrived { set; get; }

        private Action<string> _logger = (string message) =>
        {
            Console.WriteLine(message);
        };

        private static readonly XmlSerializer OrderSerializer = new XmlSerializer(typeof(Order));

        public async Task<bool> Connect(Uri peerUri)
        {
            try
            {
                ConnectionLogger?.Invoke($"Establishing connection to {peerUri.OriginalString}");

                await WebSocketClient.Connect(peerUri, ConnectionLogger!);

                return await Task.FromResult(true);
            }
            catch (WebSocketException e)
            {
                Console.WriteLine($"Caught web socket exception {e.Message}");

                ConnectionLogger?.Invoke(e.Message);

                return await Task.FromResult(false);
            }
        }

        public async Task Disconnect()
        {
            if (WebSocketClient.CurrentConnection == null)
            {
                ConnectionLogger?.Invoke("No connection to server.");
                await Task.FromResult(false);
                return;
            }

            await WebSocketClient.Disconnect();
        }

        public async Task FetchItems()
        {
            if (WebSocketClient.CurrentConnection == null)
            {
                ConnectionLogger?.Invoke("No connection to server.");
                await Task.FromResult(false);
                return;
            }
            string requestXml = "<GetAllItemsRequest/>";
            
            ConnectionLogger?.Invoke($"Sending request: {requestXml}");
            await WebSocketClient.CurrentConnection.SendAsync(requestXml);
        }

        public async Task FetchCarts()
        {
            if (WebSocketClient.CurrentConnection == null)
            {
                ConnectionLogger?.Invoke("No connection to server.");
                await Task.FromResult(false);
                return;
            }
            string requestXml = "<GetAllCartsRequest/>";
            ConnectionLogger?.Invoke($"Sending request: {requestXml}");
            await WebSocketClient.CurrentConnection.SendAsync(requestXml);
        }

        public async Task FetchCustomers()
        {
            if (WebSocketClient.CurrentConnection == null)
            {
                ConnectionLogger?.Invoke("No connection to server.");
                await Task.FromResult(false);
                return;
            }
            string requestXml = "<GetAllCustomersRequest/>";

            ConnectionLogger?.Invoke($"Sending request: {requestXml}");
            await WebSocketClient.CurrentConnection.SendAsync(requestXml);
        }

        public async Task CreateOrder(Guid id, Guid buyerId, IEnumerable<Guid> itemIds)
        {
            if (WebSocketClient.CurrentConnection == null)
            {
                ConnectionLogger?.Invoke("No connection to server.");
                await Task.FromResult(false);
                return;
            }

            Order orderRequest = new Order
            {
                Id = id,
                Buyer = new Customer { Id = buyerId },
            };
            foreach (Guid itemId in itemIds)
            {
                orderRequest.ItemsToBuy.Add(new Item { Id = itemId });
            }

            string orderXml;
            try
            {
                using (StringWriter stringWriter = new StringWriter())
                using (XmlWriter xmlWriter = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");

                    OrderSerializer.Serialize(xmlWriter, orderRequest, namespaces);
                    orderXml = stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                ConnectionLogger?.Invoke($"[ERROR] Failed to serialize Order request: {ex.Message}");
                return;
            }

            ConnectionLogger?.Invoke($"Sending Order request XML: {orderXml.Substring(0, Math.Min(orderXml.Length, 200))}");
            await WebSocketClient.CurrentConnection.SendAsync(orderXml);
        }
    }
}
