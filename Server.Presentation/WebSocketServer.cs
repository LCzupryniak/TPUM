using System.Net;
using System.Net.WebSockets;
using System.Text;
using Logic.API;
using System.Threading;

namespace Server.Presentation
{
    public class WebSocketServer
    {
        private readonly int _port;

        public WebSocketServer(int port)
        {
            _port = port;
        }
        public async Task StartAsync()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/ws/");
            listener.Start();

            Console.WriteLine("Serwer WebSocket połączono");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    _ = HandleWebSocketAsync(context); 
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private async Task HandleWebSocketAsync(HttpListenerContext context)
        {
            HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
            WebSocket webSocket = wsContext.WebSocket;

            Console.WriteLine("Połączono z klientem!");

            byte[] buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(segment, CancellationToken.None);

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Otrzymano zamówienie:\n{message}");
            }

            Console.WriteLine("Rozłączono klienta.");
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Zamknięcie", CancellationToken.None);
        }
    }
}
