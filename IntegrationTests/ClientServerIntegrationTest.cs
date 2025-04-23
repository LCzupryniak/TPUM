using ServerWebSocketConnection = Server.ObjectModels.WebSocket.WebSocketConnection;
using ClientWebSocketConnection = Client.ObjectModels.WebSocket.WebSocketConnection;
using Server.Presentation;
using Client.Data.Websocket;

namespace IntegrationTests
{
    [TestClass]
    public sealed class ClientServerIntegrationTest
    {
        [TestMethod]
        public async Task ClientServerConnectionTest()
        {
            ServerWebSocketConnection wserver = null!;
            ClientWebSocketConnection wclient = null!;
            const int delay = 10;

            Uri uri = new Uri("ws://localhost:6966/ws");
            List<string> logOutput = new List<string>();
            Task server = Task.Run(async () => await WebSocketServer.Server(uri.Port,
                _ws =>
                {
                    wserver = _ws; wserver.onMessage = (data) =>
                    {
                        logOutput.Add($"Received message from client: {data}");
                    };
                }));

            await Task.Delay(delay);

            wclient = await WebSocketClient.Connect(uri, message => logOutput.Add(message));

            Assert.IsNotNull(wserver);
            Assert.IsNotNull(wclient);

            Task clientSendTask = wclient.SendAsync("test");

            Assert.IsTrue(clientSendTask.Wait(new TimeSpan(0, 0, 1)));

            await Task.Delay(delay);

            Assert.AreEqual($"Received message from client: test", logOutput[1]);

            wclient.onMessage = (data) =>
            {
                logOutput.Add($"Received message from server: {data}");
            };

            Task serverSendTask = wserver.SendAsync("test 2");

            Assert.IsTrue(serverSendTask.Wait(new TimeSpan(0, 0, 1)));

            await Task.Delay(delay);

            Assert.AreEqual($"Received message from server: test 2", logOutput[2]);

            await wclient?.DisconnectAsync()!;
            await wserver?.DisconnectAsync()!;
        }
    }
}
