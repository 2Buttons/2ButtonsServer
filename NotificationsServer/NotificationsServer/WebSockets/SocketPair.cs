using System.Net.WebSockets;

namespace NotificationServer.WebSockets
{
  public class SocketPair
  {
    public int UserId { get; set; }
    public WebSocket WebSocket { get; set; }

    public SocketPair(int userId, WebSocket socket)
    {
      UserId = userId;
      WebSocket = socket;
    }
  }
}