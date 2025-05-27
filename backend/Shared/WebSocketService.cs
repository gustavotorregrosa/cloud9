using System.Net.WebSockets;
using System.Text;

namespace backend.Shared{
    public class WebSocketService{
      
        public void BroadcastMessage(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);
            foreach (var socket in _sockets.ToList())
            {
                if (socket.State == WebSocketState.Open)
                {
                    socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private List<WebSocket> _sockets = new List<WebSocket>();
        public void AddSocket(WebSocket socket)
        {
            // Console.WriteLine("Adding socket");
            _sockets.Add(socket);
        }

        public void RemoveSocket(WebSocket socket)
        {
            // Console.WriteLine("Removing socket");
            _sockets.Remove(socket);
        }

        public async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            AddSocket(webSocket);
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = null;
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        RemoveSocket(webSocket);
                    }
                    else
                    {
                        var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Received message: {message} from {webSocket.GetHashCode()}");
                        foreach (var socket in _sockets.ToList())
                        {
                            if (socket != webSocket && socket.State == WebSocketState.Open)
                            {
                                await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                            }
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {
                RemoveSocket(webSocket);
            }
        }   
    
    }
}