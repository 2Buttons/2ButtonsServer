using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationsData;

namespace NotificationsServer.WebSockets
{
  public class WebSocketsController
  {
    private readonly NotificationsDataUnitOfWork _db;
    private readonly NotificationManager _notificationManager;
    private readonly ILogger<NotificationManager> _logger;

    public WebSocketsController(NotificationsDataUnitOfWork db, NotificationManager notificationManager, ILogger<NotificationManager> logger)
    {
      _db = db;
      _notificationManager = notificationManager;
      _logger = logger;
    }

    public virtual async Task OnConnected(int userId, WebSocket socket)
    {
      _logger.LogInformation($"{nameof(WebSocketsController)}.{nameof(OnConnected)}.Start UserId: {userId}");
      if (!_notificationManager.IsAlreadyOpen(userId)) await SendFirstNotifications(userId, socket);
      _notificationManager.AddSocket(userId, socket);
      _logger.LogInformation($"{nameof(WebSocketsController)}.{nameof(OnConnected)}.End");
    }

    public virtual async Task OnDisconnected(WebSocket socket)
    {
      _logger.LogInformation($"{nameof(WebSocketsController)}.{nameof(OnDisconnected)}.Start");
      _notificationManager.RemoveSocket(socket);
      _logger.LogInformation($"{nameof(WebSocketsController)}.{nameof(OnDisconnected)}.End");
    }

    public async Task SendFirstNotifications(int userId, WebSocket socket)
    {
      _logger.LogInformation($"{nameof(WebSocketsController)}.{nameof(SendFirstNotifications)}.Start UserId:{userId}");
      var notifications = await _db.Notifications.GetNotifications(userId);
      await _db.Notifications.UpdateLastSeen(userId);
      if (notifications.Count > 0)
        await _notificationManager.SendMessageAsync(socket, JsonConvert.SerializeObject(notifications));
      _logger.LogInformation($"{nameof(WebSocketsController)}.{nameof(SendFirstNotifications)}.End");

    }
  }
}