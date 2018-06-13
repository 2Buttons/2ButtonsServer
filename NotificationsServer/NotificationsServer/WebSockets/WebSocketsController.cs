using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotificationsData;

namespace NotificationsServer.WebSockets
{
  public class WebSocketsController
  {
    private readonly NotificationsDataUnitOfWork _db;
    private readonly NotificationManager _notificationManager;

    public WebSocketsController(NotificationsDataUnitOfWork db, NotificationManager notificationManager)
    {
      _db = db;
      _notificationManager = notificationManager;
    }

    public virtual async Task OnConnected(int userId, WebSocket socket)
    {
      if (!_notificationManager.IsAlreadyOpen(userId)) await SendFirstNotifications(userId, socket);
      _notificationManager.AddSocket(userId, socket);
    }

    public virtual async Task OnDisconnected(WebSocket socket)
    {
      _notificationManager.RemoveSocket(socket);
    }

    public async Task SendFirstNotifications(int userId, WebSocket socket)
    {
      var notifications = await _db.Notifications.GetNotifications(userId);
      await _db.Notifications.UpdateLastSeen(userId);
      if (notifications.Count > 0)
        await _notificationManager.SendMessageAsync(socket, JsonConvert.SerializeObject(notifications));
    }
  }
}