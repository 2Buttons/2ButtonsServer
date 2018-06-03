using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationServer.WebSockets
{
  public class WebSocketConnectionManager
  {
    private readonly List<SocketPair> _sockets = new List<SocketPair>();

    public event Action<int> AddNewConnection;

    public WebSocketConnectionManager() { }

    public virtual async Task OnConnected(int userId, WebSocket socket)
    {
      var isFirstAdded = _sockets.All(x => x.UserId != userId);
      _sockets.Add(new SocketPair(userId, socket));
      if (isFirstAdded) AddNewConnection?.Invoke(userId);
    }

    public virtual async Task OnDisconnected(WebSocket socket)
    {
      var socketInList = _sockets.FirstOrDefault(x => x.WebSocket == socket);
      if (socketInList == null) return;
      _sockets.Remove(socketInList);
    }

    public async Task SendMessageAsync(WebSocket socket, string message)
    {
      if (socket.State != WebSocketState.Open) return;
      var bytes = Encoding.UTF8.GetBytes(message);
      await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true,
        CancellationToken.None);
    }

    public async Task SendMessageAsync(IEnumerable<WebSocket> sockets, string message)
    {
      foreach (var socket in sockets) await SendMessageAsync(socket, message);
    }

    public async Task SendNotificationAsync(int userId, string notification)
    {
      await SendMessageAsync(_sockets.Where(x => x.UserId == userId).Select(x => x.WebSocket).ToList(), notification);
    }

    public async Task SendMessageToAllAsync(string notification)
    {
      await SendMessageAsync(_sockets.Select(x => x.WebSocket).ToList(), notification);
    }
  }
}