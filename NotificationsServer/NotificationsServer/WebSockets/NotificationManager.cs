using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationServer.Models;

namespace NotificationServer.WebSockets
{
  public class NotificationManager
  {

    public readonly ConcurrentQueue<NotificationPair> Notifications = new ConcurrentQueue<NotificationPair>();

    public event Action AddedNotification;

    public void AddNotification(NotificationPair notification)
    {
      Notifications.Enqueue(notification);
      AddedNotification?.Invoke();
    }
  }
}
