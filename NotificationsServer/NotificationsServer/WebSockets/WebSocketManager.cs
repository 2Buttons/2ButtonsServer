using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotificationsData;
using NotificationServer.Models;

namespace NotificationServer.WebSockets
{
  public class WebSocketManager
  {
    private readonly WebSocketConnectionManager _connectionManager;

    private readonly ConcurrentQueue<NotificationPair> _notifications = new ConcurrentQueue<NotificationPair>();

    public event Action<int> SendInitializedNotification;

    private IServiceProvider _serviceProvider;
    public WebSocketManager(WebSocketConnectionManager connectionManager, IServiceProvider serviceProvider)
    {
      _connectionManager = connectionManager;
      _connectionManager.AddNewConnection += SendNotificationForNewConnection;
      _serviceProvider = serviceProvider;
      SendNotifications();

      
    }

    public void AddNotification(NotificationPair notification)
    {
      _notifications.Enqueue(notification);
    }

    public async void SendNotifications()
    {

      while (_notifications.TryDequeue(out var notificationPair))
        await _connectionManager.SendNotificationAsync(notificationPair.SendToId,
          JsonConvert.SerializeObject(notificationPair.Notification));
    }

    public async void SendNotificationForNewConnection(int userId)
    {
      //foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
      //{
      //  if (type.GetTypeInfo().BaseType == typeof(NotificationsDataUnitOfWork))
      //  {
          var db = _serviceProvider.GetService(typeof(NotificationsDataUnitOfWork));
          var notifications = await ((NotificationsDataUnitOfWork)db).Notifications.GetNotifications(userId);
          await _connectionManager.SendNotificationAsync(userId, JsonConvert.SerializeObject(notifications));
          return;
      //  }
      //}

      
    }
  }
}