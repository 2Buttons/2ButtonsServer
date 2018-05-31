using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationsServer.WebSockets
{
  public class WebSocketConnectionManager
  {
    private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    public WebSocket GetSocketById(string id)
    {
      return _sockets.FirstOrDefault(x => x.Key == id).Value;
    }

    public ConcurrentDictionary<string, WebSocket> GetAll()
    {
      return _sockets;
    }

    public string GetId(WebSocket socket)
    {
      return _sockets.FirstOrDefault(x => x.Value == socket).Key;
    }

    public bool AddSocket(WebSocket socket)
    {
      return _sockets.TryAdd(CreateConnectionId(), socket);
    }

    public async Task RemoveSocket(string id)
    {
      _sockets.TryRemove(id, out WebSocket socket);
      await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketManager",
          CancellationToken.None);
    }

    private string CreateConnectionId()
    {
      return Guid.NewGuid().ToString();
    }
  }
}
