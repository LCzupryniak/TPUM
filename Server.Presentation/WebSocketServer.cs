﻿using Server.ObjectModels.WebSocket;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace Server.Presentation
{
    public static class WebSocketServer
    {
        public static async Task Server(int p2p_port, Action<WebSocketConnection> onConnection)
        {
            Uri _uri = new Uri($@"http://localhost:{p2p_port}/");
            await ServerLoop(_uri, onConnection);
        }

        private static async Task ServerLoop(Uri _uri, Action<WebSocketConnection> onConnection)
        {
            HttpListener _server = new HttpListener();
            _server.Prefixes.Add(_uri.ToString());
            _server.Start();

            while (true)
            {
                HttpListenerContext _hc = await _server.GetContextAsync();

                if (!_hc.Request.IsWebSocketRequest)
                {
                    _hc.Response.StatusCode = 400;
                    _hc.Response.Close();
                }

                HttpListenerWebSocketContext _context = await _hc.AcceptWebSocketAsync(null);

                WebSocketConnection ws = new ServerWebSocketConnection(_context.WebSocket, _hc.Request.RemoteEndPoint);

                onConnection?.Invoke(ws);
            }
        }

        private class ServerWebSocketConnection : WebSocketConnection
        {
            public ServerWebSocketConnection(WebSocket webSocket, IPEndPoint remoteEndPoint)
            {
                m_WebSocket = webSocket;
                m_remoteEndPoint = remoteEndPoint;

                Task.Factory.StartNew(() => ServerMessageLoop(webSocket));
            }

            protected override Task SendTask(string message)
            {
                return m_WebSocket.SendAsync(message.GetArraySegment(), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            private bool isRunning = true;

            public override Task DisconnectAsync()
            {
                isRunning = false;

                if (!isRunning)
                    return m_WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutdown procedure started", CancellationToken.None);

                return Task.CompletedTask;
            }

            public override string ToString()
            {
                return m_remoteEndPoint.ToString();
            }

            private WebSocket m_WebSocket = null!;
            private IPEndPoint m_remoteEndPoint;

            private void ServerMessageLoop(WebSocket ws)
            {
                byte[] buffer = new byte[1024 * 24]; // 24KB buffer

                while (isRunning)
                {
                    ArraySegment<byte> _segments = new ArraySegment<byte>(buffer);
                    WebSocketReceiveResult _receiveResult = ws.ReceiveAsync(_segments, CancellationToken.None).Result;

                    if (_receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        onClose?.Invoke();
                        ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "I am closing", CancellationToken.None);

                        return;
                    }

                    int count = _receiveResult.Count;

                    while (!_receiveResult.EndOfMessage)
                    {
                        if (count >= buffer.Length)
                        {
                            onClose?.Invoke();
                            ws.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long", CancellationToken.None);

                            return;
                        }

                        _segments = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                        _receiveResult = ws.ReceiveAsync(_segments, CancellationToken.None).Result;

                        count += _receiveResult.Count;
                    }

                    string _message = Encoding.UTF8.GetString(buffer, 0, count);

                    onMessage?.Invoke(_message);
                }
            }
        }

        private static ArraySegment<byte> GetArraySegment(this string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return new ArraySegment<byte>(buffer);
        }
    }
}
