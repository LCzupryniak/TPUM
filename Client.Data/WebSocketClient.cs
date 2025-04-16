using System.Net.WebSockets;
using System.Text;

namespace Client.Data
{
    public class WebSocketClient
    {
        private readonly ClientWebSocket _client;

        public WebSocketClient()
        {
            _client = new ClientWebSocket();
        }

        public async Task ConnectAsync(string uri)
        {
            await _client.ConnectAsync(new Uri(uri), CancellationToken.None);
        }

        public async Task SendMessageAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(bytes);
            await _client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

    }

}
