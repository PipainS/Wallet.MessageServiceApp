    using System.Net.WebSockets;
    using System.Collections.Concurrent;

    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

        public WebSocket GetSocketById(string id) => _sockets.FirstOrDefault(p => p.Key == id).Value;

        public ConcurrentDictionary<string, WebSocket> GetAll() => _sockets;

        public string AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);
            return id;
        }

        public async Task RemoveSocket(string id)
        {
            if (_sockets.TryRemove(id, out var socket) && socket != null)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket connection closed", CancellationToken.None);
                socket.Dispose();
            }
        }

    }
