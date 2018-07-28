using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationsServer.Models;

namespace NotificationsServer.WebSockets
{
  public class NotificationManager
  {

    private readonly ConcurrentQueue<NotificationPair> _notifications = new ConcurrentQueue<NotificationPair>();
    private readonly List<SocketPair> _sockets = new List<SocketPair>();
    private readonly ILogger<NotificationManager> _logger;
    public NotificationManager(ILogger<NotificationManager> logger)
    {
      _logger = logger;
    }

   // public event Action AddedNotification +=  CheckNotifications;

    public async Task AddNotification(NotificationPair notification)
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(AddNotification)}.Start");
      _notifications.Enqueue(notification);
      await CheckNotifications();
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(AddNotification)}.End");
      //AddedNotification?.Invoke();
    }

    public bool IsAlreadyOpen(int userId)
    {
      _logger.LogInformation($"{nameof(WebSocketsController)}.{nameof(IsAlreadyOpen)}");
      return _sockets.Any(x => x.UserId == userId && x.WebSocket.State == System.Net.WebSockets.WebSocketState.Open);
    }

    public void AddSocket(int userId, WebSocket socket)
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(AddSocket)}.Start");
      _sockets.Add(new SocketPair(userId, socket));
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(AddSocket)}.End");
    }

    public void RemoveSocket(WebSocket socket)
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(RemoveSocket)}.Start");
      var sp = _sockets.FirstOrDefault(x => x.WebSocket == socket);
      if (sp == null) return;
      _sockets.Remove(sp);
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(RemoveSocket)}.End");
    }

    public void CleanUpSockets()
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(CleanUpSockets)}.Start");
      var sockets = _sockets.Where(x => x.WebSocket.State != WebSocketState.Open).ToList();
      foreach (var socket in sockets)
      {
        _sockets.Remove(socket);
      }
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(CleanUpSockets)}.End");
    }

    public async Task CheckNotifications()
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(CheckNotifications)}.Start");
      while (_notifications.TryDequeue(out var notification))
      {
       await  SendNotificationAsync(notification.SendToId, JsonConvert.SerializeObject(notification.Notification));
      }
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(CheckNotifications)}.End");
    }


    public async Task SendMessageAsync(WebSocket socket, string message)
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(SendMessageAsync)}.Start");
      if (socket.State != WebSocketState.Open) return;
      var bytes = Encoding.UTF8.GetBytes(message);
      await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true,
        CancellationToken.None);
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(SendMessageAsync)}.End");
    }

    public async Task SendNotificationAsync(int userId, string notification)
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(SendNotificationAsync)}.Start");
      var sockets = _sockets.Where(x => x.UserId == userId).Select(x => x.WebSocket).ToList();
      foreach (var socket in sockets) await SendMessageAsync(socket, notification);
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(SendNotificationAsync)}.End");
    }

    public async Task SendMessageToAllAsync(string notification)
    {
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(SendMessageToAllAsync)}.Start");
      foreach (var socket in _sockets.ToList()) await SendMessageAsync(socket.WebSocket, notification);
      _logger.LogInformation($"{nameof(NotificationManager)}.{nameof(SendMessageToAllAsync)}.End");
    }

  }
}
