using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonLibraries;
using CommonLibraries.Helpers;
using Newtonsoft.Json;
using NotificationServer.Models;

namespace NotificationServer.WebSockets
{
  public class NotificationManager
  {

    private readonly ConcurrentQueue<NotificationPair> _notifications = new ConcurrentQueue<NotificationPair>();
    private readonly List<SocketPair> _sockets = new List<SocketPair>();

   // public event Action AddedNotification +=  CheckNotifications;

    public async Task AddNotification(NotificationPair notification)
    {
      _notifications.Enqueue(notification);
      await CheckNotifications();
      //AddedNotification?.Invoke();
    }

    public bool IsAlreadyOpen(int userId)
    {
      return _sockets.Any(x => x.UserId == userId && x.WebSocket.State == System.Net.WebSockets.WebSocketState.Open);
    }

    public void AddSocket(int userId, WebSocket socket)
    {
      _sockets.Add(new SocketPair(userId, socket));
    }

    public void RemoveSocket(WebSocket socket)
    {
      var sp = _sockets.FirstOrDefault(x => x.WebSocket == socket);
      if (sp == null) return;
      _sockets.Remove(sp);
    }

    public void CleanUpSockets()
    {
      var sockets = _sockets.Where(x => x.WebSocket.State != WebSocketState.Open).ToList();
      foreach (var socket in sockets)
      {
        _sockets.Remove(socket);
      }
    }

    public async Task CheckNotifications()
    {
      while (_notifications.TryDequeue(out var notification))
      {
       await  SendNotificationAsync(notification.SendToId, JsonConvert.SerializeObject(notification.Notification));
      }
    }


    public async Task SendMessageAsync(WebSocket socket, string message)
    {

      if (socket.State != WebSocketState.Open) return;
      var bytes = Encoding.UTF8.GetBytes(message);
      await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true,
        CancellationToken.None);

    }

    public async Task SendNotificationAsync(int userId, string notification)
    {
      var sockets = _sockets.Where(x => x.UserId == userId).Select(x => x.WebSocket).ToList();
      foreach (var socket in sockets) await SendMessageAsync(socket, notification);
    }

    public async Task SendMessageToAllAsync(string notification)
    {
      foreach (var socket in _sockets.ToList()) await SendMessageAsync(socket.WebSocket, notification);
    }

  }
}
