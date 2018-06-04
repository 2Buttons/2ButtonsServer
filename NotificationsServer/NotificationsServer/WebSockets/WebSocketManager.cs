using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotificationsData;
using NotificationsData.Account;
using NotificationsData.Main;
using NotificationServer.Models;

namespace NotificationServer.WebSockets
{
  public class WebSocketManager
  {
    private readonly WebSocketConnectionManager _connectionManager;

    private readonly ConcurrentQueue<NotificationPair> _notifications = new ConcurrentQueue<NotificationPair>();

    public event Action<int> SendInitializedNotification;

    private IServiceProvider _serviceProvider;
    public WebSocketManager()
    {
      // _connectionManager = connectionManager;
      // _connectionManager.AddNewConnection += SendNotificationForNewConnection;
      // SendNotifications();


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

      //var p = _serviceProvider.GetService(typeof(TwoButtonsAccountContext));
      //var z = _serviceProvider.GetService(typeof(TwoButtonsContext));
      //var db = _serviceProvider.GetService(typeof(NotificationsDataUnitOfWork));
      //    var notifications = await ((NotificationsDataUnitOfWork)db).Notifications.GetNotifications(userId);
      //    await _connectionManager.SendNotificationAsync(userId, JsonConvert.SerializeObject(notifications));
      //    return;
      //  }
      //}


    }
  }
}